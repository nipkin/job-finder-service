namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillAreaRequest(string Name, ICollection<string> Skills, int SkillWeight);
}
