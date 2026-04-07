namespace JobFinder.Application.UserProfile.UserSkills
{
    public class UserSkillService(IUserProfileRepository repository) : IUserSkillService
    {
        public async Task<Result<UserSkillResult>> AddSkill(Guid userId, Guid areaId, string name)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = user.UserSkillAreas.FirstOrDefault(a => a.Id == areaId);
            if (area is null)
                return Error.NotFound($"Skill area '{areaId}' not found.");

            var error = area.AddSkill(name);
            if (error is not null) return Error.Validation(error);

            await repository.SaveAsync();

            var skill = area.UserSkills.First(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
            return new UserSkillResult(skill.Id, skill.Name);
        }

        public async Task<Result<UserSkillResult>> UpdateSkill(Guid userId, Guid skillId, string name)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = user.UserSkillAreas.FirstOrDefault(a => a.UserSkills.Any(s => s.Id == skillId));
            if (area is null)
                return Error.NotFound($"Skill '{skillId}' not found.");

            var error = area.UpdateSkill(skillId, name);
            if (error is not null) return Error.Validation(error);

            await repository.SaveAsync();

            var skill = area.UserSkills.First(s => s.Id == skillId);
            return new UserSkillResult(skill.Id, skill.Name);
        }

        public async Task<Result> RemoveSkill(Guid userId, Guid skillId)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = user.UserSkillAreas.FirstOrDefault(a => a.UserSkills.Any(s => s.Id == skillId));
            if (area is null)
                return Error.NotFound($"Skill '{skillId}' not found.");

            var error = area.RemoveSkill(skillId);
            if (error is not null) return Error.Validation(error);

            await repository.SaveAsync();
            return Result.Ok();
        }
    }
}