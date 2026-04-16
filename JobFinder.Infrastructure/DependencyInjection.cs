using JobFinder.Application.Auth;
using JobFinder.Application.Common;
using JobFinder.Application.JobPostings;
using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using JobFinder.Infrastructure.Auth;
using JobFinder.Infrastructure.JobPostings;
using JobFinder.Infrastructure.JobScoring;
using JobFinder.Infrastructure.JobSearch;
using JobFinder.Infrastructure.UserProfile;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobFinder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IJobMatchRunner, JobMatchRunner>();
            services.AddScoped<IJobPostingWriter, JobPostingWriter>();
            services.AddScoped<IJobPostingReader, JobPostingReader>();
            services.AddScoped<IUserProfileRepository, UserProfileRepository>();
            services.AddScoped<IAuthRepository, AuthRepository>();

            services.AddHttpClient<IJobSearchApiClient, JobSearchApiClient>();

            var mistralSection = configuration.GetSection(MistralOptions.SectionName);
            if (mistralSection.Exists() && !string.IsNullOrWhiteSpace(mistralSection["ApiKey"]))
            {
                services.Configure<MistralOptions>(mistralSection);
                services.AddHttpClient<IJobScoringClient, RemoteJobScoringClient>(http =>
                {
                    http.BaseAddress = new Uri("https://api.mistral.ai");
                });
            }
            else
            {
                services.AddHttpClient<IJobScoringClient, LocalJobScoringClient>(http =>
                {
                    http.BaseAddress = new Uri(configuration["OllamaBaseUrl"]!);
                    http.Timeout = TimeSpan.FromSeconds(120);
                });
            }

            return services;
        }
    }
}
