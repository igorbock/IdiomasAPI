namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnswerController : Controller
{
    private readonly ICRUDService<Answer> _service;

    public AnswerController(ICRUDService<Answer> service)
    {
        _service = service;
    }

    [HttpGet("answered", Name = "Pendentes")]
    [SwaggerOperation("Obter respostas não corrigidas.")]
    public async Task<IQueryable<Answer>> Answered() => await _service.GetAllAsync();

    [HttpPost("answer", Name = "Responder")]
    [SwaggerOperation("Criar resposta para pergunta.")]
    public async Task AnswerIt(Answer answer) => await _service.CreateAsync(answer);

    [HttpPut]
    [SwaggerOperation("Corrigir resposta.")]
    public async Task Correct(Answer answer) => await _service.EditAsync(answer);
}
