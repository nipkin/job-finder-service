using JobFinder.Application.Auth;
using JobFinder.Application.JobPostings;
using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using JobFinder.Application.UserProfile;
using JobFinder.Application.UserProfile.UserSkillAreas;
using JobFinder.Application.UserProfile.UserSkills;
using Microsoft.Extensions.DependencyInjection;

namespace JobFinder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IJobPostingImportService, JobPostingImportService>();
            services.AddScoped<IJobScoringService, JobScoringService>();
            services.AddScoped<IUserProfileService, UserProfileService>();
            services.AddScoped<IUserSkillService, UserSkillService>();
            services.AddScoped<IUserSkillAreaService, UserSkillAreaService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJobSearchTermsService, JobSearchTermsService>();
            return services;
        }

        public static IServiceCollection AddApplicationApi(this IServiceCollection services)
        {
            services.AddApplication();
            services.AddScoped<IJobPostingProviderService, JobPostingProviderService>();
            return services;
        }
    }
}
