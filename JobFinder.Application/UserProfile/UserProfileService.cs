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

            var skills = userProfile.UserSkills
                .Select(s => new UserProfileSkillAreaResponse(s.Name, s.Skill, s.SkillWeight))
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

            var userSkillArea = new UserSkillArea { Name = request.Name, Skill = request.Skills, SkillWeight = request.SkillWeight };
            user.UserSkills.Add(userSkillArea);
            await repository.SaveAsync();

            return new UserProfileSkillAreaResponse(userSkillArea.Name, userSkillArea.Skill, userSkillArea.SkillWeight);
        }

        public async Task<Result<string>> AddSkill(UserProfileSkillRequest request)
        {
            if (string.IsNullOrEmpty(request.Skill))
                return Error.Validation("Skill is required.");

            var user = await repository.GetByIdAsync(request.UserId);
            if (user is null)
                return Error.NotFound($"User '{request.UserId}' not found.");

            var skill = user.UserSkills.FirstOrDefault(skill => skill.Id == request.SkillId);
            if (skill is null)
                return Error.NotFound($"Skill area '{request.SkillId}' not found.");

            skill.Skill.Add(request.Skill);
            await repository.SaveAsync();

            return request.Skill;
        }
    }
}