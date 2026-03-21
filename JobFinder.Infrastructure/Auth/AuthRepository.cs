using JobFinder.Application.Auth;
using JobFinder.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Infrastructure.Auth
{
    public class AuthRepository(JobFinderDbContext db) : IAuthRepository
    {
        public async Task<DomainEntities.UserProfile?> GetByUserNameAsync(string userName, CancellationToken ct = default)
        {
            return await db.UserProfiles
                .FirstOrDefaultAsync(u => u.UserName == userName, ct);
        }

        public void Add(DomainEntities.UserProfile userProfile)
        {
            db.UserProfiles.Add(userProfile);
        }

        public async Task SaveAsync(CancellationToken ct = default)
        {
            await db.SaveChangesAsync(ct);
        }
    }
}
