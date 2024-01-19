namespace IdiomasAPI.Services;

public class ImageService : IService<Image, ChatResponse>
{
    private readonly HttpClient? _httpClient;
    private readonly IHttpResponseMethods<ChatResponse> _bardResponse;

    public ImageService(
        IHttpClientFactory httpClientFactory, 
        IHttpResponseMethods<ChatResponse> bardResponse)
    {
        _httpClient = httpClientFactory.CreateClient("GoogleAPI");
        _bardResponse = bardResponse;
    }

    public async Task<ChatResponse> Get(Image requestEntity)
    {
        if (string.IsNullOrEmpty(requestEntity.Base64String))
            throw new ArgumentNullException(nameof(requestEntity.Base64String));

        var entity = new
        {
            contents = new[]
            {
            new
            {
                parts = new dynamic[]
                {
                    new
                    {
                        text = "O que há nessa imagem?"
                    },
                    new
                    {
                        inline_data = new
                        {
                            mime_type = "image/jpeg",
                            data = requestEntity.Base64String
                        }
                    }
                }
            }
        }
        };

        var json = JsonSerializer.Serialize(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient!.PostAsync($"v1beta/models/gemini-pro-vision:generateContent?key={"AIzaSyDD8_0O2SApQm7wWzvWg_p-geFsACtPIAE"}", content);
        response.EnsureSuccessStatusCode();

        return await _bardResponse.GetHttpResponseTypeT(response);
    }
}
