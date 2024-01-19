namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class QuestionController : Controller
{
    private readonly ICRUDService<Question> _service;

    public QuestionController(ICRUDService<Question> service)
    {
        _service = service;
    }

    [HttpGet]
    [SwaggerOperation("Obter perguntas aleatórias, baseado na unidade informada.")]
    public async Task<IEnumerable<Question>> Get(int unit) => await _service.GetAllAsync(unit);

    [HttpPost]
    [SwaggerOperation("Incluir pergunta na base de dados.")]
    public async Task<int> Create(Question question) => await _service.CreateAsync(question);
}
