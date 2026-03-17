using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingWriter
    {
        void Remove(JobPosting posting);
        void Add(JobPosting posting);
        Task SaveAsync(CancellationToken ct = default);
        Task<int> RemoveOutdatedAsync(CancellationToken ct = default);
        Task<bool> JobPostExists(string postingUrl, CancellationToken ct = default);
    }
}
