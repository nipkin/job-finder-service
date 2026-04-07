namespace JobFinder.Application.UserProfile.UserSkillAreas
{
    public interface IUserSkillAreaService
    {
        Task<Result<UserSkillAreaResult>> CreateSkillAreaAsync(Guid userId, AddUserSkillAreaCommand request);
        Task<Result<UserSkillAreaResult>> UpdateSkillAreaAsync(Guid userId, Guid areaId, UpdateUserSkillAreaCommand command);
        Task<Result> DeleteSkillAreaAsync(Guid userId, Guid areaId);
    }
}
