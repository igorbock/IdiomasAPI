namespace IdiomasAPI.Context;

public class IdiomasContext : DbContext
{
    public IdiomasContext(DbContextOptions<IdiomasContext> options) : base(options) { }

    public DbSet<Glossary> Glossaries { get; set; }
    public DbSet<Question> Questions { get; set; }
}
