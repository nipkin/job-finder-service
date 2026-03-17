namespace JobFinder.Application.JobSearch
{
    public class JobSearchService(IJobSearchApiClient client) : IJobSearchService
    {
        public async Task<List<JobSearchResult>> SearchAllAsync(IEnumerable<string> terms, CancellationToken ct)
        {
            var results = await Task.WhenAll(terms.Select(t => client.SearchAsync(t, ct)));
            return results.SelectMany(r => r).ToList();
        }
    }
}