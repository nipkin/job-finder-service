namespace JobFinder.Application.JobSearch
{
    public interface IJobSearchApiClient
    {
        Task<List<JobSearchResult>> SearchAsync(string query, CancellationToken ct);
    }
}
