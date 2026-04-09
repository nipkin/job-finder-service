using JobFinder.Application.JobPostings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Channels;

namespace JobFinder.Api.Features.JobMatch
{
    [ApiController]
    [Authorize]
    [Route("api/jobmatch")]
    public class JobMatchController(
        IJobPostingImportService importService,
        IJobMatchRunner runner) : ControllerBase
    {
        private Guid UserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        [HttpGet("status")]
        public IActionResult GetStatus() => Ok(new { isRunning = runner.IsRunning(UserId) });

        [HttpGet("run")]
        public async Task RunAsync(CancellationToken ct)
        {
            Response.ContentType = "text/event-stream";
            Response.Headers["Cache-Control"] = "no-cache";
            Response.Headers["X-Accel-Buffering"] = "no";

            if (runner.IsRunning(UserId))
            {
                await WriteEventAsync("error", "A job is already running.", ct);
                return;
            }

            if (!runner.TryStart(UserId, out var cts))
            {
                await WriteEventAsync("error", "A job is already running.", ct);
                return;
            }

            var channel = Channel.CreateUnbounded<string>();
            var progress = new Progress<string>(msg => channel.Writer.TryWrite(msg));

            using var linkedCts = CancellationTokenSource.CreateLinkedTokenSource(ct, cts.Token);

            _ = Task.Run(async () =>
            {
                try
                {
                    await importService.ImportJobPostingsAsync(
                        UserId,
                        progress,
                        linkedCts.Token);

                    channel.Writer.TryWrite("__done__");
                }
                catch (OperationCanceledException)
                {
                    channel.Writer.TryWrite("__cancelled__");
                }
                catch (Exception ex)
                {
                    channel.Writer.TryWrite($"__error__{ex.Message}");
                }
                finally
                {
                    runner.Complete(UserId);
                    channel.Writer.Complete();
                }
            }, CancellationToken.None);

            try
            {
                await foreach (var message in channel.Reader.ReadAllAsync(ct))
                {
                    if (message == "__done__")
                    {
                        await WriteEventAsync("done", "Job completed.", ct);
                        break;
                    }
                    if (message == "__cancelled__")
                    {
                        await WriteEventAsync("cancelled", "Job was stopped.", ct);
                        break;
                    }
                    if (message.StartsWith("__error__"))
                    {
                        await WriteEventAsync("error", message[9..], ct);
                        break;
                    }

                    await WriteEventAsync("progress", message, ct);
                }
            }
            catch (OperationCanceledException)
            {
                // client disconnected — background job continues until runner.Complete
            }
        }

        [HttpPost("stop")]
        public IActionResult Stop()
        {
            runner.Stop(UserId);
            return NoContent();
        }

        private async Task WriteEventAsync(string eventType, string data, CancellationToken ct)
        {
            await Response.WriteAsync($"event: {eventType}\ndata: {data}\n\n", ct);
            await Response.Body.FlushAsync(ct);
        }
    }
}
