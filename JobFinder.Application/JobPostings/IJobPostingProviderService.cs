namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingProviderService
    {
        Task<IReadOnlyList<JobPostingResult>> GetAllPostingsAsync();
        Task<IReadOnlyList<JobPostingResult>> GetUserPostingsAsync(Guid userId, CancellationToken ct = default);
    }
}
