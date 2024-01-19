namespace IdiomasLIB.Models;

[Table("answer")]
public class Answer
{
    [Key]
    public int Id { get; set; }
    public bool? IsCorrect { get; set; }
    public string? Text { get; set; }

    [ForeignKey(nameof(Question))]
    public int QuestionId { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.Always)]
    public virtual Question? Question { get; set; }
}
