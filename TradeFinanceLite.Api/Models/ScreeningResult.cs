public class ScreeningResult
{
    public int Id { get; set; }
    public int LcId { get; set; }
    public LetterOfCredit Lc { get; set; } = null!;
    public string MatchedName { get; set; } = string.Empty;
    public int MatchScore { get; set; }
    public bool IsFlagged { get; set; }
    public DateTime CheckedAt { get; set; } = DateTime.UtcNow;
}