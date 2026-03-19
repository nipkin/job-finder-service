using JobFinder.Application.JobSearch;
using System.Text.Json;

namespace JobFinder.Infrastructure.JobSearch
{
    public class JobSearchApiClient(HttpClient client) : IJobSearchApiClient
    {
        public async Task<IReadOnlyList<JobSearchResult>> SearchAsync(string query, CancellationToken ct)
        {
            var url = $"https://jobsearch.api.jobtechdev.se/search?q={Uri.EscapeDataString(query)}";

            var response = await client.GetAsync(url, ct);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync(ct);
            var data = JsonSerializer.Deserialize<JobSearchResponse>(json);

            return data?.Hits.Select(x => x.ToApplication()).ToList() ?? [];
        }
    }
}