namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GlossaryController : Controller
{
    private readonly ICRUDService<Glossary> _service;

    public GlossaryController(ICRUDService<Glossary> service)
    {
        _service = service;
    }

    [HttpGet]
    [SwaggerOperation("Obter glossário, baseado na unidade informada.")]
    public async Task<IQueryable<Glossary>> GetAll(int? unit) => await _service.GetAllAsync(unit);

    [HttpPost]
    [SwaggerOperation("Inserir nova palavra no glossário.")]
    public async Task Create(Glossary glossary) => await _service.CreateAsync(glossary);

    [HttpPost("all")]
    [SwaggerOperation("Inserir várias palavras no glossário.")]
    public async Task CreateAll(IEnumerable<Glossary>? glossaries)
    {
        if (glossaries == null)
            throw new JsonException("Nenhum glossário no conteúdo!");

        await _service.CreateAllAsync(glossaries);
    }
}
