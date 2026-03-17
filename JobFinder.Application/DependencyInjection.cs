using JobFinder.Application.JobPostings;
using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using Microsoft.Extensions.DependencyInjection;

namespace JobFinder.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IJobPostingImportService, JobPostingImportService>();
            services.AddScoped<IJobScoringService, JobScoringService>();
            services.AddScoped<IJobSearchService, JobSearchService>();
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
