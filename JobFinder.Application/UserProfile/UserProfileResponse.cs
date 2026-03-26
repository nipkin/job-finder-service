namespace JobFinder.Application.UserProfile
{
    public record UserProfileResponse(
        Guid Id,
        string UserName,
        ICollection<UserProfileSkillAreaResponse> UserSkills,
        ICollection<UserProfileJobPostingResponse> JobPostings);
}