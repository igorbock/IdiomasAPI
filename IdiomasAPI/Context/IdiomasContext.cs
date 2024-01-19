namespace IdiomasAPI.Context;

public class IdiomasContext : DbContext
{
    public IdiomasContext(DbContextOptions<IdiomasContext> options) : base(options) { }

    public virtual DbSet<Glossary> Glossaries { get; set; }
    public virtual DbSet<Question> Questions { get; set; }
    public virtual DbSet<Answer> Answers { get; set; }
}
