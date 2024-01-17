namespace IdiomasLIB.Chat;

public class Parts
{
    [JsonPropertyName("parts")]
    public IEnumerable<Text>? Partes { get; set; }
}
