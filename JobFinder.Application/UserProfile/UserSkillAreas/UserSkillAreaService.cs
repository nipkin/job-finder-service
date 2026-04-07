using JobFinder.Application.UserProfile.UserSkills;
using JobFinder.Domain.Entities;

namespace JobFinder.Application.UserProfile.UserSkillAreas
{
    internal class UserSkillAreaService(IUserProfileRepository repository) : IUserSkillAreaService
    {
        public async Task<Result<UserSkillAreaResult>> CreateSkillAreaAsync(Guid userId, AddUserSkillAreaCommand request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return Error.Validation("Skill area name is required.");

            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = new UserSkillArea { Id = Guid.NewGuid(), Name = request.Name, SkillWeight = request.SkillWeight };

            foreach (var skillName in request.Skills)
            {
                var error = area.AddSkill(skillName);
                if (error is not null) return Error.Validation(error);
            }

            user.UserSkillAreas.Add(area);
            await repository.SaveAsync();

            var skills = area.UserSkills.Select(s => new UserSkillResult(s.Id, s.Name)).ToList();
            return new UserSkillAreaResult(area.Id, area.Name, skills, area.SkillWeight);
        }

        public async Task<Result<UserSkillAreaResult>> UpdateSkillAreaAsync(Guid userId, Guid areaId, UpdateUserSkillAreaCommand command)
        {
            if (string.IsNullOrEmpty(command.Name))
                return Error.Validation("Skill area name is required.");

            if (command.SkillWeight < 1 || command.SkillWeight > 5)
                return Error.Validation("Skill weight must be between 1 and 5.");

            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = user.UserSkillAreas.FirstOrDefault(a => a.Id == areaId);
            if (area is null)
                return Error.NotFound($"Skill area '{areaId}' not found.");

            area.Name = command.Name;
            area.SkillWeight = command.SkillWeight;
            await repository.SaveAsync();

            var skills = area.UserSkills.Select(s => new UserSkillResult(s.Id, s.Name)).ToList();
            return new UserSkillAreaResult(area.Id, area.Name, skills, area.SkillWeight);
        }

        public async Task<Result> DeleteSkillAreaAsync(Guid userId, Guid areaId)
        {
            var user = await repository.GetByIdAsync(userId);
            if (user is null)
                return Error.NotFound($"User '{userId}' not found.");

            var area = user.UserSkillAreas.FirstOrDefault(a => a.Id == areaId);
            if (area is null)
                return Error.NotFound($"Skill area '{areaId}' not found.");

            user.UserSkillAreas.Remove(area);
            await repository.SaveAsync();

            return Result.Ok();
        }
    }
}
