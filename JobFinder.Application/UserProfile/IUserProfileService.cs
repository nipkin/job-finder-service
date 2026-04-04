namespace JobFinder.Application.UserProfile
{
    public interface IUserProfileService
    {
        Task<Result<UserProfileResponse>> GetByIdAsync(Guid id, CancellationToken ct = default);
        Task<Result<UserProfileSkillAreaResponse>> CreateSkillArea(UserProfileSkillAreaRequest request);
        Task<Result> DeleteSkillArea(Guid userId, Guid areaId);
        Task<Result<SkillResponse>> AddSkill(Guid userId, Guid areaId, string name);
        Task<Result<SkillResponse>> UpdateSkill(Guid userId, Guid skillId, string name);
        Task<Result> RemoveSkill(Guid userId, Guid skillId);
    }
}
