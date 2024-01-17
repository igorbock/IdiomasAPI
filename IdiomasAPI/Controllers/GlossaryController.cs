namespace IdiomasAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GlossaryController : Controller
{
    private readonly IdiomasContext _context;

    public GlossaryController(IdiomasContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task Create(Glossary glossary)
    {
        await _context.Glossaries.AddAsync(glossary);
        await _context.SaveChangesAsync();
    }

    [HttpPost("all")]
    public async Task CreateAll(IEnumerable<Glossary>? glossaries)
    {
        if (glossaries == null)
            throw new JsonException("Nenhum glossário no conteúdo!");

        await _context.Glossaries.AddRangeAsync(glossaries);
        await _context.SaveChangesAsync();
    }

    [HttpGet]
    public Task<IQueryable<Glossary>> GetAll(int unit) => Task.FromResult(_context.Glossaries.Where(a => a.Unit == unit));
}
