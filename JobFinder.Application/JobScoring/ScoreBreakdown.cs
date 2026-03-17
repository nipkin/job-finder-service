using System.Text.Json.Serialization;

namespace JobFinder.Application.JobScoring
{
    public class ScoreBreakdown
    {
        [JsonPropertyName("core")]
        public int Core { get; set; }

        [JsonPropertyName("frontend")]
        public int Frontend { get; set; }

        [JsonPropertyName("cms")]
        public int Cms { get; set; }

        [JsonPropertyName("experience")]
        public int Experience { get; set; }

        [JsonPropertyName("seniority")]
        public int Seniority { get; set; }
    }
}
