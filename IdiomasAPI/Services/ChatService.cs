namespace IdiomasAPI.Services;

public class ChatService : IService<GeminiPro, GeminiProResponse>
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private List<Content> _contents = new List<Content>();

    public ChatService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("GoogleAPI");
        _apiKey = configuration["API_KEY"]!.ToString();
    }

    public void Dispose() => _contents.Clear();

    public async Task<GeminiProResponse> Get(GeminiPro requestEntity)
    {
        _contents.AddRange(requestEntity.Contents!);

        requestEntity.Contents = _contents;

        var json = JsonSerializer.Serialize(requestEntity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"v1beta/models/gemini-pro:generateContent?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();

        var geminiProResponse = await response.Content.ReadFromJsonAsync<GeminiProResponse>();
        _contents.Add(geminiProResponse!.Candidates![0].Content!);

        return geminiProResponse ?? throw new Exception("Deu errado!");
    }
}
