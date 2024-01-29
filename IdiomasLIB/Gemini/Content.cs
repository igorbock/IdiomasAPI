namespace IdiomasLIB.Gemini;

public class Content
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    [JsonPropertyName("role")]
    public string? Role { get; set; }

    [JsonPropertyName("parts")]
    public List<object>? Parts { get; set; }    
}
