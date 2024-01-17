namespace IdiomasLIB;

[Table("glossary")]
public class Glossary
{
    [Key]
    public int Id { get; set; }

    public int Unit { get; set; }

    [MaxLength(30)]
    public string? Word { get; set; }

    [MaxLength(30)]
    public string? Translate { get; set; }

}
