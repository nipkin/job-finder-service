namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillAreaResponse(Guid Id, string Name, ICollection<UserSkillResponse> Skills, int SkillWeight);
}
