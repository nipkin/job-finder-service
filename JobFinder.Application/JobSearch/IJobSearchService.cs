namespace JobFinder.Application.JobSearch
{
    public interface IJobSearchService
    {
        Task<List<JobSearchResult>> SearchAllAsync(IEnumerable<string> terms, CancellationToken ct);
    }
}
