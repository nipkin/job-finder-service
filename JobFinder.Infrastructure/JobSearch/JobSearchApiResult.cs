using System.Text.Json.Serialization;

namespace JobFinder.Infrastructure.JobSearch
{
    public class JobSearchApiResult
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("headline")]
        public string Headline { get; set; } = string.Empty;

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;

        [JsonPropertyName("webpage_url")]
        public string WebpageUrl { get; set; } = string.Empty;

        [JsonPropertyName("application_deadline")]
        public DateTime? ApplicationDeadline { get; set; }

        [JsonPropertyName("number_of_vacancies")]
        public int NumberOfVacancies { get; set; }

        [JsonPropertyName("relevance")]
        public double Relevance { get; set; }

        [JsonPropertyName("description")]
        public JobDescription Description { get; set; } = new();

        [JsonPropertyName("workplace_address")]
        public WorkplaceAddress WorkplaceAddress { get; set; } = new();
    }

    public class WorkplaceAddress
    {

        [JsonPropertyName("region")]
        public string Region { get; set; } = string.Empty;
    }

    public class JobDescription
    {
        [JsonPropertyName("text")]
        public string Text { get; set; } = string.Empty;
    }
}
