namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingProviderService
    {
        Task<IReadOnlyList<JobPostingResult>> GetAllPostingsAsync();
    }
}
