namespace IdiomasLIB.Gemini;

public class InlineData
{
    [JsonPropertyName("mime_type")]
    public string? MimeType { get; set; }

    [JsonPropertyName("data")]
    public string? Data { get; set; }
}
