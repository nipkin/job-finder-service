using JobFinder.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace JobFinder.Infrastructure.Data
{
    public class JobFinderDbContext(DbContextOptions<JobFinderDbContext> options) : DbContext(options)
    {
        public DbSet<JobPosting> JobPostings => Set<JobPosting>();
    }
}
