using JobFinder.Domain.Entities;

namespace JobFinder.Application.UserProfile
{
    public class UserProfileService(IUserProfileRepository repository) : IUserProfileService
    {
        public async Task<Result<UserProfileResponse>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var userProfile = await repository.GetByIdAsync(id, ct);
            if (userProfile is null)
                return Error.NotFound($"User '{id}' not found.");

            var skills = userProfile.UserSkillAreas
                .Select(s => new UserProfileSkillAreaResponse(s.Id, s.Name, s.UserSkills.Select(sk => new SkillResponse(sk.Id, sk.Name)).ToList(), s.SkillWeight))
                .ToList();

            var jobPostings = userProfile.JobPostings
                .Select(j => new UserProfileJobPostingResponse(j.Id, j.Headline, j.Region, j.ApplicationDeadline, j.WebpageUrl, j.CreatedAtUtc, j.CvScore))
                .ToList();

            return new UserProfileResponse(userProfile.Id, userProfile.UserName, skills, jobPostings);
        }

        public async Task<Result<UserProfileSkillAreaResponse>> CreateSkillArea(UserProfileSkillAreaRequest request)
        {
            if (string.IsNullOrEmpty(request.Name))
                return Error.Validation("Skill area name is required.");

            var user = await repository.GetByIdAsync(request.UserId);
            if (user is null)
                return Error.NotFound($"User '{request.UserId}' not found.");

            var area = new UserSkillArea { Id = Guid.NewGuid(), Name = request.Name, SkillWeight = request.SkillWeight };

            foreach (var skillName in request.Skills)
            {
                var error = area.AddSkill(skillName);
                if (error is not null) return Error.Validation(error);
            }

            user.UserSkillAreas.Add(area);
            await repository.SaveAsync();

            var skills = area.UserSkills.Select(s => new SkillResponse(s.Id, s.Name)).ToList();
            return new UserProfileSkillAreaResponse(area.Id, area.Name, skills, area.SkillWeight);
        }

        public async Task<Result> DeleteSkillArea(Guid userId, Guid areaId)
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

        public async Task<Result<SkillResponse>> AddSkill(Guid userId, Guid areaId, string name)
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
            return new SkillResponse(skill.Id, skill.Name);
        }

        public async Task<Result<SkillResponse>> UpdateSkill(Guid userId, Guid skillId, string name)
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
            return new SkillResponse(skill.Id, skill.Name);
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
