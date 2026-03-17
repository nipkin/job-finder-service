using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobPostings
{
    public interface IJobPostingReader
    {
        Task<List<JobPosting>> GetAllAsync();
    }
}
