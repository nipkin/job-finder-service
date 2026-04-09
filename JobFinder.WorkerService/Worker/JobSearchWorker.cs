using JobFinder.Application.Common;
using JobFinder.Application.JobPostings;
using JobFinder.WorkerService.Configuration;
using Microsoft.Extensions.Options;

namespace JobFinder.WorkerService.Worker;

public class JobSearchWorker(
IOptions<SearchOptions> options,
IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly SearchOptions _options = options.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var userProfileRepo = scope.ServiceProvider.GetRequiredService<IUserProfileRepository>();
        var importService = scope.ServiceProvider.GetRequiredService<IJobPostingImportService>();

        var userProfile = await userProfileRepo.GetByIdAsync(_options.UserProfileId, ct);
        if (userProfile is null) return;

        await importService.ImportJobPostingsAsync(_options.UserProfileId, ct: ct);
    }
}
