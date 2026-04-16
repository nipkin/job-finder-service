using JobFinder.Application.Common;
using JobFinder.Application.UserProfile.UserSkillAreas;
using JobFinder.Application.UserProfile.UserSkills;

namespace JobFinder.Application.UserProfile
{
    public class UserProfileService(IUserProfileRepository repository) : IUserProfileService
    {
        public async Task<Result<UserProfileResult>> GetByIdAsync(Guid id, CancellationToken ct = default)
        {
            var userProfile = await repository.GetByIdAsync(id, ct);
            if (userProfile is null)
                return Error.NotFound($"User '{id}' not found.");

            var skills = userProfile.UserSkillAreas
                .Select(s => new UserSkillAreaResult(s.Id, s.Name, s.UserSkills.Select(sk => new UserSkillResult(sk.Id, sk.Name)).ToList(), s.SkillWeight))
                .ToList();

            var jobPostings = userProfile.JobPostings
                .Select(j => new UserJobPostingResult(j.Id, j.Headline, j.Region, j.ApplicationDeadline, j.WebpageUrl, j.CreatedAtUtc, j.CvScore))
                .ToList();

            return new UserProfileResult(userProfile.Id, userProfile.UserName, skills, jobPostings);
        }

        public async Task<Result<UserCvResult>> UpdateUserCvAsync(Guid id, string cvText, CancellationToken ct = default)
        {
            if (string.IsNullOrEmpty(cvText))
                return Error.NotFound($"CV cannot be empty");

            var userProfile = await repository.GetByIdAsync(id, ct);
            if (userProfile is null)
                return Error.NotFound($"User '{id}' not found.");

            userProfile.CvText = cvText;
            await repository.SaveAsync(ct);

            return new UserCvResult(id, cvText);
        }

        public async Task<Result<UserCvResult>> GetUserCvAsync(Guid id, CancellationToken ct = default)
        {
            var cvText = await repository.GetUserCvAsync(id, ct);
            return new UserCvResult(id, cvText ?? string.Empty);
        }
    }
}