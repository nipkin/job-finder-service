using JobFinder.Application.JobPostings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace JobFinder.Api.Features.JobPostings
{
    [ApiController]
    [Route("api/[controller]")]
    public class JobPostingController(IJobPostingProviderService provider) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var results = await provider.GetAllPostingsAsync();
            return Ok(results);
        }

        [Authorize]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyPostingsAsync(CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var results = await provider.GetUserPostingsAsync(userId, ct);
            return Ok(results);
        }
    }
}
