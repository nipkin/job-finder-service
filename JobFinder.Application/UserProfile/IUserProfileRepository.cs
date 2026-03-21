using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileRepository
    {
        Task<DomainEntities.UserProfile?> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task SaveAsync(CancellationToken ct = default);
    }
}
