namespace JobFinder.Infrastructure.JobScoring
{
    public class MistralOptions
    {
        public const string SectionName = "Mistral";

        public string ApiKey { get; set; } = "";
        public string Model { get; set; } = "mistral-small-latest";
    }
}
