using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LcController : ControllerBase
{
    private readonly AppDbContext _db;

    public LcController(AppDbContext db)
    {
        _db = db;
    }

    private int CurrentUserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

    // ---------- CREATE (Maker only) ----------
    [HttpPost]
    [Authorize(Roles = "Maker")]
    public async Task<IActionResult> Create(CreateLcRequest request)
    {
        var lc = new LetterOfCredit
        {
            LcNumber = $"LC-{DateTime.UtcNow:yyyyMMddHHmmss}",
            ApplicantName = request.ApplicantName,
            BeneficiaryName = request.BeneficiaryName,
            Amount = request.Amount,
            Currency = request.Currency,
            IssueDate = request.IssueDate,
            ExpiryDate = request.ExpiryDate,
            Terms = request.Terms,
            Status = LcStatus.PendingApproval,
            CreatedByUserId = CurrentUserId
        };

        _db.LettersOfCredit.Add(lc);
        await _db.SaveChangesAsync();

        // --- Screening ---
        var (isFlagged, matchedName, score) = NameScreeningService.Screen(lc.BeneficiaryName);

        _db.ScreeningResults.Add(new ScreeningResult
        {
            LcId = lc.Id,
            MatchedName = matchedName ?? "No match",
            MatchScore = score,
            IsFlagged = isFlagged
        });
        await _db.SaveChangesAsync();

        await LogAudit("LetterOfCredit", lc.Id, "Created",
            isFlagged
                ? $"LC {lc.LcNumber} created. FLAGGED — matched '{matchedName}' ({score}%)."
                : $"LC {lc.LcNumber} created and sent for approval.");

        await _db.Entry(lc).Reference(l => l.CreatedBy).LoadAsync();

        return Ok(MapToResponse(lc));
    }

    // ---------- GET ALL ----------
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var lcs = await _db.LettersOfCredit
            .Include(l => l.CreatedBy)
            .Include(l => l.ApprovedBy)
            .ToListAsync();

        return Ok(lcs.Select(MapToResponse));
    }

    // ---------- GET BY ID ----------
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var lc = await _db.LettersOfCredit
            .Include(l => l.CreatedBy)
            .Include(l => l.ApprovedBy)
            .FirstOrDefaultAsync(l => l.Id == id);

        if (lc == null) return NotFound();

        return Ok(MapToResponse(lc));
    }

    // ---------- APPROVE (Checker only) ----------
    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Checker")]
    public async Task<IActionResult> Approve(int id, ApproveRejectRequest request)
    {
        var lc = await _db.LettersOfCredit.FindAsync(id);
        if (lc == null) return NotFound();

        if (lc.Status != LcStatus.PendingApproval)
            return BadRequest("Only LCs pending approval can be approved.");

        if (lc.CreatedByUserId == CurrentUserId)
            return BadRequest("Maker cannot approve their own LC.");

        lc.Status = LcStatus.Approved;
        lc.Remarks = request.RemarksOrReason;
        lc.ApprovedByUserId = CurrentUserId;
        await _db.SaveChangesAsync();

        await LogAudit("LetterOfCredit", lc.Id, "Approved", request.RemarksOrReason);

        return Ok(MapToResponse(lc));
    }

    // ---------- REJECT (Checker only) ----------
    [HttpPost("{id}/reject")]
    [Authorize(Roles = "Checker")]
    public async Task<IActionResult> Reject(int id, ApproveRejectRequest request)
    {
        var lc = await _db.LettersOfCredit.FindAsync(id);
        if (lc == null) return NotFound();

        if (lc.Status != LcStatus.PendingApproval)
            return BadRequest("Only LCs pending approval can be rejected.");

        lc.Status = LcStatus.Rejected;
        lc.Remarks = request.RemarksOrReason;
        lc.ApprovedByUserId = CurrentUserId;
        await _db.SaveChangesAsync();

        await LogAudit("LetterOfCredit", lc.Id, "Rejected", request.RemarksOrReason);

        return Ok(MapToResponse(lc));
    }

    // ---------- Helpers ----------
    private async Task LogAudit(string entityName, int entityId, string action, string? details)
    {
        _db.AuditLogs.Add(new AuditLog
        {
            EntityName = entityName,
            EntityId = entityId,
            Action = action,
            PerformedByUserId = CurrentUserId,
            Details = details
        });
        await _db.SaveChangesAsync();
    }


    private LcResponse MapToResponse(LetterOfCredit lc)
    {
        var screening = _db.ScreeningResults
            .Where(s => s.LcId == lc.Id)
            .OrderByDescending(s => s.CheckedAt)
            .FirstOrDefault();

        return new LcResponse(
            lc.Id,
            lc.LcNumber,
            lc.ApplicantName,
            lc.BeneficiaryName,
            lc.Amount,
            lc.Currency,
            lc.IssueDate,
            lc.ExpiryDate,
            lc.Terms,
            lc.Status.ToString(),
            lc.CreatedBy?.FullName ?? "",
            lc.ApprovedBy?.FullName,
            screening?.IsFlagged ?? false,
            screening?.MatchScore ?? 0,
            lc.Remarks
        );
    }
}