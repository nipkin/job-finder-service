using JobFinder.Application.UserProfile.UserSkillAreas;

namespace JobFinder.Application.UserProfile
{
    public record UserProfileResult(
        Guid Id,
        string UserName,
        ICollection<UserSkillAreaResult> UserSkills,
        ICollection<UserJobPostingResult> JobPostings);
}