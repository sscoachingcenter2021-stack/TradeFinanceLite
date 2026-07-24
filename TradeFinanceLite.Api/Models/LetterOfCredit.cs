public enum LcStatus { Draft, PendingApproval, Approved, Rejected, Active, Expired }

public class LetterOfCredit
{
    public int Id { get; set; }
    public string LcNumber { get; set; } = string.Empty;
    public string ApplicantName { get; set; } = string.Empty;
    public string BeneficiaryName { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
    public DateTime IssueDate { get; set; }
    public DateTime ExpiryDate { get; set; }
    public string Terms { get; set; } = string.Empty;
    public LcStatus Status { get; set; } = LcStatus.Draft;

    public int CreatedByUserId { get; set; }
    public User CreatedBy { get; set; } = null!;

    public int? ApprovedByUserId { get; set; }
    public User? ApprovedBy { get; set; }

    public ICollection<ScreeningResult> ScreeningResults { get; set; } = new List<ScreeningResult>();
}