using Microsoft.EntityFrameworkCore;

namespace JobFinder.Infrastructure.Data
{
    public class JobFinderDbContext(DbContextOptions<JobFinderDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.JobPosting> JobPostings => Set<Domain.Entities.JobPosting>();
    }
}
