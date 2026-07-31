public record CreateLcRequest(
    string ApplicantName,
    string BeneficiaryName,
    decimal Amount,
    string Currency,
    DateTime IssueDate,
    DateTime ExpiryDate,
    string Terms
);

public record LcResponse(
    int Id,
    string LcNumber,
    string ApplicantName,
    string BeneficiaryName,
    decimal Amount,
    string Currency,
    DateTime IssueDate,
    DateTime ExpiryDate,
    string Terms,
    string Status,
    string CreatedByName,
    string? ApprovedByName,
    bool IsFlagged,
    int ScreeningScore,
    string? Remarks
);

public record ApproveRejectRequest(string? RemarksOrReason);