namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingImportService
    {
        Task ImportJobPostingsAsync(Guid userId, IProgress<string>? progress = null, CancellationToken ct = default);
    }
}
