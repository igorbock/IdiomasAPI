namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : Controller
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;
    private readonly IdiomasContext _context;
    private string _apiKey;

    public QuestionController(
        IConfiguration configuration, 
        IHttpClientFactory httpClientFactory,
        IdiomasContext context)
    {
        _configuration = configuration;
        _httpClient = httpClientFactory.CreateClient("GoogleAPI");
        _apiKey = _configuration["API_KEY"]!.ToString();
        _context = context;
    }

    ~QuestionController()
    {
        _httpClient.Dispose();
    }

    [HttpGet(Name = "Aleatórias")]
    public async Task<IEnumerable<Question>> Create(int unit)
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

        var response = await _httpClient.PostAsync($"https://generativelanguage.googleapis.com/v1beta/models/gemini-pro:generateContent?key={_apiKey}", content);
        response.EnsureSuccessStatusCode();

        var responseJson = await response.Content.ReadAsStringAsync();
        var jObject = JObject.Parse(responseJson);
        var text = (string)jObject.SelectToken("candidates[0].content.parts[0].text")!;

        var retorno = new List<Question>();
        var questions = text.Split("\n").Where(a => a.Contains('?'));
        foreach (var item in questions)
        {
            var question = item.Split('.');
            retorno.Add(new Question(int.Parse(question[0]), question[1].Trim()));
        }

        return retorno;
    }

    [HttpPost("answer", Name = "Responder")]
    public async Task AnswerIt(Question question)
    {
        if(string.IsNullOrEmpty(question.Answer))
            throw new ArgumentNullException(nameof(question.Answer));

        await _context.Questions.AddAsync(question);
        await _context.SaveChangesAsync();
    }

    [HttpPut(Name = "Corrigir")]
    public async Task Correct(int id, bool correct)
    {
        var question = _context.Questions.First(a => a.Id == id);
        if (question is null)
            throw new KeyNotFoundException();

        question.IsCorrect = correct;
        await _context.SaveChangesAsync();
    }

    [HttpGet("answered", Name = "Pendentes")]
    public Task<IQueryable<Question>> Answered() => Task.FromResult(_context.Questions.Where(a => a.IsCorrect == null));
}
