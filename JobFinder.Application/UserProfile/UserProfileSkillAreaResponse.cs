namespace JobFinder.Application.UserProfile
{
    public record UserProfileSkillAreaResponse(string Name, ICollection<string> Skills, int SkillWeight);
}
