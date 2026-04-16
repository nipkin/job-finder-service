using JobFinder.Application.Common;
using JobFinder.Application.UserProfile.UserSkills;

namespace JobFinder.Application.UserProfile.UserSkillAreas
{
    internal class UserSkillAreaService(IUserProfileRepository repository) : IUserSkillAreaService
    {
        public async Task<Result<UserSkillAreaResult>> CreateSkillAreaAsync(Guid userId, AddUserSkillAreaCommand request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return Error.Validation("Skill area name is required.");

            var user = await repository.GetProfileShallowAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var (area, areaError) = user.AddSkillArea(request.Name, request.SkillWeight);
            if (areaError is not null) return Error.Validation(areaError);

            foreach (var skillName in request.Skills)
            {
                var error = area!.AddSkill(skillName);
                if (error is not null) return Error.Validation(error);
            }
            await repository.SaveAsync();

            var skills = area.UserSkills.Select(s => new UserSkillResult(s.Id, s.Name)).ToList();
            return new UserSkillAreaResult(area.Id, area.Name, skills, area.SkillWeight);
        }

        public async Task<Result<UserSkillAreaResult>> UpdateSkillAreaAsync(Guid userId, Guid areaId, UpdateUserSkillAreaCommand command)
        {
            if (string.IsNullOrEmpty(command.Name))
                return Error.Validation("Skill area name is required.");

            var user = await repository.GetProfileWithSkillAreaAsync(userId, areaId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var error = user.UpdateSkillArea(areaId, command.Name, command.SkillWeight);
            if (error is not null) return Error.Validation(error);

            await repository.SaveAsync();

            var area = user.UserSkillAreas.First(a => a.Id == areaId);
            var skills = area.UserSkills.Select(s => new UserSkillResult(s.Id, s.Name)).ToList();
            return new UserSkillAreaResult(area.Id, area.Name, skills, area.SkillWeight);
        }

        public async Task<Result> DeleteSkillAreaAsync(Guid userId, Guid areaId)
        {
            var user = await repository.GetProfileWithSkillAreaAsync(userId, areaId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var error = user.RemoveSkillArea(areaId);
            if (error is not null) return Error.NotFound(error);

            await repository.SaveAsync();

            return Result.Ok();
        }
    }
}
