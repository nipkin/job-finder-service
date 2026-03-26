namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillAreaRequest(Guid UserId, string Name, ICollection<string> Skills, int SkillWeight);
}
