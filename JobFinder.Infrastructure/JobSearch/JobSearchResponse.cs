using System.Text.Json.Serialization;

namespace JobFinder.Infrastructure.JobSearch
{
    public class JobSearchResponse
    {
        [JsonPropertyName("total")]
        public Total Total { get; set; } = new();

        [JsonPropertyName("positions")]
        public int Positions { get; set; }

        [JsonPropertyName("query_time_in_millis")]
        public int QueryTimeInMillis { get; set; }

        [JsonPropertyName("result_time_in_millis")]
        public int ResultTimeInMillis { get; set; }

        [JsonPropertyName("hits")]
        public List<JobSearchApiResult> Hits { get; set; } = [];
    }

    public class Total
    {
        [JsonPropertyName("value")]
        public int Value { get; set; }
    }
}