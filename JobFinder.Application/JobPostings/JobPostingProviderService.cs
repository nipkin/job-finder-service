namespace JobFinder.Application.JobPostings
{
    public class JobPostingProviderService(IJobPostingReader reader) : IJobPostingProviderService
    {
        public async Task<IReadOnlyList<JobPostingResult>> GetAllPostingsAsync()
        {
            var results = await reader.GetAllAsync();
            return results.Select(JobPostingMapper.ToResponse).ToList();
        }

        public async Task<IReadOnlyList<JobPostingResult>> GetUserPostingsAsync(Guid userId, CancellationToken ct = default)
        {
            var results = await reader.GetByUserIdAsync(userId, ct);
            return results.Select(JobPostingMapper.ToResponse).ToList();
        }
    }
}