namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<UserProfileSkillAreaResponse>> CreateSkillArea(Guid userId, UserProfileSkillAreaRequest request);
        Task<Result> DeleteSkillArea(Guid userId, Guid areaId);
        Task<Result<UserSkillResponse>> AddSkill(Guid userId, Guid areaId, string name);
        Task<Result<UserSkillResponse>> UpdateSkill(Guid userId, Guid skillId, string name);
        Task<Result> RemoveSkill(Guid userId, Guid skillId);
    }
}
