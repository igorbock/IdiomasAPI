namespace IdiomasLIB.Chat;

public class RequestModel
{
    [JsonPropertyName("contents")]
    public IEnumerable<Parts>? Conteudo { get; set; }
}