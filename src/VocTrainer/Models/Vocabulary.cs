namespace VocTrainer.Models;

public class Vocabulary
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public string German { get; set; } = string.Empty;
    public string English { get; set; } = string.Empty;
}