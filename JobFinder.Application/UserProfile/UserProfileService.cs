using JobFinder.Domain.Entities;

namespace JobFinder.Application.UserProfile
{
    public class UserProfileService(IUserProfileRepository repository) : IUserProfileService
    {
        public async Task<UserProfileResponse?> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var userProfile = await repository.GetByIdAsync(id, ct);

            if (userProfile is null)
                return null;

            var skills = userProfile.UserSkills
                .Select(s => new UserProfileSkillAreaResponse(s.Name, s.Skill, s.SkillWeight))
                .ToList();

            var jobPostings = userProfile.JobPostings
                .Select(j => new UserProfileJobPostingResponse(j.Id, j.Headline, j.Region, j.ApplicationDeadline, j.WebpageUrl, j.CreatedAtUtc, j.CvScore))
                .ToList();

            return new UserProfileResponse(userProfile.Id, userProfile.UserName, skills, jobPostings);
        }

        public async Task<UserProfileSkillAreaResponse?> CreateSkillArea(UserProfileSkillAreaRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.Name)) 
                return null;

            var user = await repository.GetByIdAsync(request.UserId);
            if(user is null) 
                return null;

            var userSkillArea = new UserSkillArea { Name = request.Name, Skill = request.Skills, SkillWeight = request.SkillWeight };
            user.UserSkills.Add(userSkillArea);
            await repository.SaveAsync();

            return new UserProfileSkillAreaResponse(userSkillArea.Name, userSkillArea.Skill, userSkillArea.SkillWeight);
        }

        public async Task<string?> AddSkill(UserProfileSkillRequest request)
        {
            if (request is null || string.IsNullOrEmpty(request.Skill))
                return null;

            var user = await repository.GetByIdAsync(request.UserId);
            if (user is null)
                return null;

            var skill = user.UserSkills.FirstOrDefault(skill => skill.Id == request.SkillId);
            if (skill is null)
                return null;

            skill.Skill.Add(request.Skill);
            await repository.SaveAsync();
            
            return request.Skill;
        }
    }
}