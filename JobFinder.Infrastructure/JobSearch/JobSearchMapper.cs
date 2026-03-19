using JobFinder.Application.JobSearch;

namespace JobFinder.Infrastructure.JobSearch
{
    public static class JobSearchMapper
    {
        public static JobSearchResult ToApplication(this JobSearchApiResult hit) => new()
        {
            Id = hit.Id,
            Headline = hit.Headline,
            Region = hit.Region,
            WebpageUrl = hit.WebpageUrl,
            ApplicationDeadline = hit.ApplicationDeadline,
            NumberOfVacancies = hit.NumberOfVacancies,
            Relevance = hit.Relevance,
            Description = new Application.JobSearch.JobDescription
            {
                Text = hit.Description.Text
            },
            WorkplaceAddress = new Application.JobSearch.WorkplaceAddress
            {
                Region = hit.WorkplaceAddress.Region
            },
        };
    }
}
