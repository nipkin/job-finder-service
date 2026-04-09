using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobSearch
{
    public interface IJobSearchTermsService
    {
        Task<Result<List<string>>> GenerateSearchTermsAsync(ICollection<UserSkillArea> userSkillAreas, CancellationToken ct = default);
    }
}
