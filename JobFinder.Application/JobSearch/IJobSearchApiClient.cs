namespace JobFinder.Application.JobSearch
{
    public interface IJobSearchApiClient
    {
        Task<IReadOnlyList<JobSearchResult>> SearchAsync(string query, CancellationToken ct);
    }
}
