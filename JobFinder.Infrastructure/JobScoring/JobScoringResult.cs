using System.Text.Json.Serialization;

namespace JobFinder.Infrastructure.JobScoring
{
    public class JobScoringResult
    {
        [JsonPropertyName("response")]
        public string Response { get; set; } = "";
    }
}