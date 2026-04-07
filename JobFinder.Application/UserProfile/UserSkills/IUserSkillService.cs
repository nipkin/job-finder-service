namespace JobFinder.Application.UserProfile.UserSkills
{
    public interface IUserSkillService
    {
        Task<Result<UserSkillResult>> AddSkill(Guid userId, Guid areaId, string name);
        Task<Result<UserSkillResult>> UpdateSkill(Guid userId, Guid skillId, string name);
        Task<Result> RemoveSkill(Guid userId, Guid skillId);
    }
}
