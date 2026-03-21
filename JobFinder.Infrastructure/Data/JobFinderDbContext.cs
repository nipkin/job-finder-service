using Microsoft.EntityFrameworkCore;

namespace JobFinder.Infrastructure.Data
{
    public class JobFinderDbContext(DbContextOptions<JobFinderDbContext> options) : DbContext(options)
    {
        public DbSet<Domain.Entities.UserProfile> UserProfiles => Set<Domain.Entities.UserProfile>();
        public DbSet<Domain.Entities.JobPosting> JobPostings => Set<Domain.Entities.JobPosting>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Domain.Entities.UserProfile>(entity => entity.HasIndex(x => x.UserName).IsUnique());
            modelBuilder.Entity<Domain.Entities.UserProfile>()
                .HasMany(u => u.JobPostings)
                .WithOne(j => j.UserProfile)
         
                .HasForeignKey(j => j.UserProfileId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
