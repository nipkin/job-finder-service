using JobFinder.Application.JobPostings;
using JobFinder.WorkerService.Configuration;
using Microsoft.Extensions.Options;

namespace JobFinder.WorkerService.Worker;

public class JobSearchWorker(
IOptions<SearchOptions> options,
IOptions<JobSkills> skills,
IServiceScopeFactory scopeFactory) : BackgroundService
{
    private readonly SearchOptions _options = options.Value;
    private readonly JobSkills _skills = skills.Value;

    protected override async Task ExecuteAsync(CancellationToken ct)
    {
        using var scope = scopeFactory.CreateScope();
        var importService = scope.ServiceProvider.GetRequiredService<IJobPostingImportService>();
        await importService.ImportJobPostingsAsync(ToScoringRequest(_skills), _options.SearchTerms, ct);
    }

    private static UserJobSkills ToScoringRequest(JobSkills jobSkills) => new()
    {
        CoreSkills = jobSkills.CoreSkills,
        CmsSkills = jobSkills.CmsSkills,
        FrontendSkills = jobSkills.FrontendSkills,
        CvText = jobSkills.CvText
    };
}