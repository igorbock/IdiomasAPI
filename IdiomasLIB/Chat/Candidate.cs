namespace IdiomasLIB.Chat;

public class Candidate
{
    [JsonPropertyName("content")]
    public RequestModel? Content { get; set; }
    public string? finishReason { get; set; }
    public int index { get; set; }
    public List<SafetyRating>? safetyRatings { get; set; }
}
