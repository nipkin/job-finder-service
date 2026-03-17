using JobFinder.Application.JobScoring;
using System.Net.Http.Json;

namespace JobFinder.Infrastructure.JobScoring
{
    public class JobScoringClient(HttpClient http) : IJobScoringClient
    {
        public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            var body = new { model = "mistral", prompt, stream = false };
            var response = await http.PostAsJsonAsync("/api/generate", body, ct);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<JobScoringResult>(ct);
            return result!.Response;
        }

        public async Task<string> GenerateJsonAsync(string prompt, CancellationToken ct = default)
        {
            var body = new { model = "mistral", prompt, stream = false, format = "json" };
            var response = await http.PostAsJsonAsync("/api/generate", body, ct);
            response.EnsureSuccessStatusCode();
            var result = await response.Content.ReadFromJsonAsync<JobScoringResult>(ct);
            return result!.Response;
        }
    }
}
