namespace IdiomasLIB;

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

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public string? Answer { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    public bool? IsCorrect { get; set; }
}
