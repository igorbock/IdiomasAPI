namespace IdiomasLIB.Gemini.Response;

public class PromptFeedback
{
    [JsonPropertyName("safetyRatings")]
    public List<SafetyRating>? SafetyRatings { get; set; }
}
