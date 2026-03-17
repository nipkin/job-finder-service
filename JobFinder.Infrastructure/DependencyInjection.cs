using JobFinder.Application.JobPostings;
using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using JobFinder.Infrastructure.Data;
using JobFinder.Infrastructure.JobPostings;
using JobFinder.Infrastructure.JobScoring;
using JobFinder.Infrastructure.JobSearch;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace JobFinder.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddScoped<IJobPostingWriter, JobPostingWriter>();
            services.AddScoped<IJobPostingReader, JobPostingReader>();

            services.AddHttpClient<IJobSearchApiClient, JobSearchApiClient>();
            services.AddHttpClient<IJobScoringClient, JobScoringClient>();

            return services;
        }
    }
}
