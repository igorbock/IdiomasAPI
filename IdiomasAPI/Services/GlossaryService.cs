namespace IdiomasAPI.Services;

public class GlossaryService : ICRUDService<Glossary>
{
    private readonly IdiomasContext _context;

    public GlossaryService(IdiomasContext context)
    {
        _context = context;
    }

    public async Task<int> CreateAsync(Glossary type)
    {
        var entity = await _context.Glossaries.AddAsync(type);
        await _context.SaveChangesAsync();

        return entity.Entity.Id;
    }

    public async Task CreateAllAsync(IEnumerable<Glossary> types)
    {
        if (types == null)
            throw new ArgumentNullException("Nenhum glossário no conteúdo!");

        await _context.Glossaries.AddRangeAsync(types);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Glossary type) => throw new NotImplementedException();

    public Task EditAsync(Glossary type) => throw new NotImplementedException();

    public Task<Glossary> GetAsync(int id) => throw new NotImplementedException();

    public async Task<IQueryable<Glossary>> GetAllAsync(int? unit)
    {
        if(unit == null)
            return (await Task.FromResult(_context.Glossaries)).AsQueryable<Glossary>();
        else
            return _context.Glossaries.Where(a => a.Unit == unit);
    }
}
