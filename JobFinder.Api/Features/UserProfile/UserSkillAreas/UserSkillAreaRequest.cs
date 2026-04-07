namespace JobFinder.Api.Features.UserProfile.UserSkillAreas
{
    public record UserSkillAreaRequest(string Name, ICollection<string> Skills, int SkillWeight);
}