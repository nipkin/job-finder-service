namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingProviderService
    {
        Task<List<JobPostingResponse>> GetAllPostingsAsync();
    }
}
