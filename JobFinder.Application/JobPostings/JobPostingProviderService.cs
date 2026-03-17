namespace JobFinder.Application.JobPostings
{
    public class JobPostingProviderService(IJobPostingReader reader) : IJobPostingProviderService
    {
        public async Task<List<JobPostingResponse>> GetAllPostingsAsync()
        {
            var results = await reader.GetAllAsync();
            return results.Select(JobPostingMapper.ToResponse).ToList();
        }
    }
}