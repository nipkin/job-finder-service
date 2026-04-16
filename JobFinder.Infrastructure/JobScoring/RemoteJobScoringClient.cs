using JobFinder.Application.JobScoring;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace JobFinder.Infrastructure.JobScoring
{
    public class RemoteJobScoringClient(HttpClient http, IOptions<MistralOptions> options) : IJobScoringClient
    {
        private readonly MistralOptions _options = options.Value;

        public async Task<string> GenerateAsync(string prompt, CancellationToken ct = default)
        {
            var request = BuildRequest(prompt, jsonMode: false);
            return await SendAsync(request, ct);
        }

        public async Task<string> GenerateJsonAsync(string prompt, CancellationToken ct = default)
        {
            var request = BuildRequest(prompt, jsonMode: true);
            return await SendAsync(request, ct);
        }

        private object BuildRequest(string prompt, bool jsonMode)
        {
            var messages = new[] { new { role = "user", content = prompt } };

            if (jsonMode)
                return new { model = _options.Model, messages, response_format = new { type = "json_object" } };

            return new { model = _options.Model, messages };
        }

        private async Task<string> SendAsync(object body, CancellationToken ct)
        {
            using var request = new HttpRequestMessage(HttpMethod.Post, "/v1/chat/completions")
            {
                Content = JsonContent.Create(body)
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", _options.ApiKey);

            var response = await http.SendAsync(request, ct);
            response.EnsureSuccessStatusCode();

            var result = await response.Content.ReadFromJsonAsync<MistralChatResponse>(ct);
            return result!.Choices[0].Message.Content;
        }
    }

    file class MistralChatResponse
    {
        [JsonPropertyName("choices")]
        public List<MistralChoice> Choices { get; set; } = [];
    }

    file class MistralChoice
    {
        [JsonPropertyName("message")]
        public MistralMessage Message { get; set; } = new();
    }

    file class MistralMessage
    {
        [JsonPropertyName("content")]
        public string Content { get; set; } = "";
    }
}
