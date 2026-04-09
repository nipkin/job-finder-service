using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingReader
    {
        Task<IReadOnlyList<JobPosting>> GetAllAsync();
        Task<IReadOnlyList<JobPosting>> GetByUserIdAsync(Guid userId, CancellationToken ct = default);
    }
}
