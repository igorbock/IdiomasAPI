namespace IdiomasAPI.Services;

public class ChatService : IService<GeminiPro, GeminiProResponse>
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public ChatService(
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration)
    {
        _httpClient = httpClientFactory.CreateClient("GoogleAPI");
        _apiKey = configuration["API_KEY"]!.ToString();
    }

    public async Task<GeminiProResponse> Get(GeminiPro requestEntity)
    {
        var json = JsonSerializer.Serialize(requestEntity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"v1beta/models/gemini-pro:generateContent?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();

        var geminiProResponse = await response.Content.ReadFromJsonAsync<GeminiProResponse>();

        return geminiProResponse ?? throw new Exception("Deu errado!");
    }
}
