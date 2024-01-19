namespace IdiomasLIB.Models;

[Table("questions")]
public class Question
{
    public Question() { }

    public Question(int number, string text)
    {
        Number = number;
        Text = text;
    }

    [Key]
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public int Id { get; set; }

    public int Number { get; set; }
    public string? Text { get; set; }
}
