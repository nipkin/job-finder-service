namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingImportService
    {
        Task ImportJobPostingsAsync(UserJobSkills userJobSkills, IEnumerable<string> terms, CancellationToken ct = default);
    }
}
