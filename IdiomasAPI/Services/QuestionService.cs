namespace IdiomasAPI.Services;

public class QuestionService : ICRUDService<Question>
{
    private readonly IdiomasContext _context;
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;
    private readonly IHttpResponseMethods<IQueryable<Question>> _httpResponseMethods;

    public QuestionService(
        IdiomasContext context,
        IHttpClientFactory httpClientFactory,
        IConfiguration configuration,
        IHttpResponseMethods<IQueryable<Question>> httpResponseMethods)
    {
        _context = context;
        _httpClient = httpClientFactory.CreateClient("GoogleAPI");
        _apiKey = configuration["API_KEY"]!.ToString();
        _httpResponseMethods = httpResponseMethods;
    }

    public Task CreateAllAsync(IEnumerable<Question> types) => throw new NotImplementedException();

    public async Task<int> CreateAsync(Question type)
    {
        var entity = await _context.Questions.AddAsync(type);
        await _context.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public Task DeleteAsync(Question type) => throw new NotImplementedException();

    public Task EditAsync(Question type) => throw new NotImplementedException();

    public async Task<IQueryable<Question>> GetAllAsync(int? unit = null)
    {
        var words = _context.Glossaries.Where(a => a.Unit <= unit).Select(a => a.Word);
        var entity = new RequestModel
        {
            Conteudo = new List<Parts>
            {
                new Parts
                {
                    Partes = new List<Text>
                    {
                        new Text { Texto = $"Crie 10 perguntas curtas, com o tempo verbal no presente, em inglês, utilizando somente as seguintes palavras: {string.Join(", ", words)}." }
                    }
                }
            }
        };

        var json = JsonSerializer.Serialize(entity);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        var response = await _httpClient.PostAsync($"v1beta/models/gemini-pro:generateContent?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();

        return await _httpResponseMethods.GetHttpResponseTypeT(response);
    }

    public Task<Question> GetAsync(int id) => throw new NotImplementedException();
}
