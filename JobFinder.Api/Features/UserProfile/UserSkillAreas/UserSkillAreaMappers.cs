using JobFinder.Application.UserProfile.UserSkillAreas;

namespace JobFinder.Api.Features.UserProfile.UserSkillAreas
{
    public static class UserSkillAreaMappers
    {
        public static AddUserSkillAreaCommand ToAddCommand(UserSkillAreaRequest request) =>
            new(request.Name, request.Skills, request.SkillWeight);

        public static UpdateUserSkillAreaCommand ToUpdateCommand(UpdateUserSkillAreaRequest request) =>
            new(request.Name, request.SkillWeight);
    }
}
