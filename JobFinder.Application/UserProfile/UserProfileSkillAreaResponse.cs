namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillAreaResponse(Guid Id, string Name, ICollection<SkillResponse> Skills, int SkillWeight);
}
