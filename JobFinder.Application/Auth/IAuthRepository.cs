using DomainEntities = JobFinder.Domain.Entities;

namespace JobFinder.Application.Auth
{
    public interface IAuthRepository
    {
        Task<DomainEntities.UserProfile?> GetByUserNameAsync(string userName, CancellationToken ct = default);
        void Add(DomainEntities.UserProfile userProfile);
        Task SaveAsync(CancellationToken ct = default);
    }
}