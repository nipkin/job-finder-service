using JobFinder.Application.JobScoring;
using JobFinder.Application.JobSearch;
using JobFinder.Domain.Entities;

namespace JobFinder.Application.JobPostings
{
    public static class JobPostingMapper
    {
        public static JobPostingResult ToResponse(JobPosting jobPosting) => new()
        {
            Id = jobPosting.Id,
            Headline = jobPosting.Headline,
            Region = jobPosting.Region,
            Description = jobPosting.Description,
            ApplicationDeadline = jobPosting.ApplicationDeadline,
            WebpageUrl = jobPosting.WebpageUrl,
            CreatedAtUtc = jobPosting.CreatedAtUtc,
            OptimizedCv = jobPosting.OptimizedCv,
            CvScore = jobPosting.CvScore
        };

        public static JobPosting FromApi(JobSearchResult api, double cvScore) => new()
        {
            Headline = api.Headline,
            Region = api.WorkplaceAddress != null ? api.WorkplaceAddress.Region : string.Empty,
            Description = api.Description != null ? api.Description.Text : string.Empty,
            ApplicationDeadline = api.ApplicationDeadline,
            WebpageUrl = api.WebpageUrl,
            CreatedAtUtc = DateTime.UtcNow,
            CvScore = cvScore,
        };
    }
}
