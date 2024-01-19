namespace IdiomasAPI.Services;

public class AnswerService : ICRUDService<Answer>
{
    private readonly IdiomasContext _context;

    public AnswerService(IdiomasContext context)
    {
        _context = context;
    }

    public Task CreateAllAsync(IEnumerable<Answer> types) => throw new NotImplementedException();

    public async Task CreateAsync(Answer type)
    {
        if (string.IsNullOrEmpty(type.Text))
            throw new ArgumentNullException(nameof(type.Text));

        await _context.Answers.AddAsync(type);
        await _context.SaveChangesAsync();
    }

    public Task DeleteAsync(Answer type) => throw new NotImplementedException();

    public async Task EditAsync(Answer type)
    {
        _context.Answers.Update(type);
        await _context.SaveChangesAsync();
    }

    public Task<IQueryable<Answer>> GetAllAsync(int? unit = null) => Task.FromResult(_context.Answers.Where(a => a.IsCorrect == null));

    public Task<Answer> GetAsync(int id) => throw new NotImplementedException();
}
