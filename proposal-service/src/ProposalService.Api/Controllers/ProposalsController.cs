using Microsoft.AspNetCore.Mvc;
using ProposalService.Api.Contracts.Proposals;
using ProposalService.Api.Storage;

namespace ProposalService.Api.Controllers;

[ApiController]
[Route("api/proposals")]
public sealed class ProposalsController : ControllerBase
{
    private readonly IProposalRepository _repo;

    public ProposalsController(IProposalRepository repo)
    {
        _repo = repo;
    }

    [HttpPost]
    public async Task<ActionResult<ProposalDto>> Create(
        [FromBody] CreateProposalRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
            return BadRequest("CustomerName is required.");

        if (request.InsuredAmount <= 0)
            return BadRequest("InsuredAmount must be greater than 0.");

        var now = DateTimeOffset.UtcNow;

        var proposal = new ProposalDto
        {
            Id = Guid.NewGuid(),
            CustomerName = request.CustomerName.Trim(),
            InsuredAmount = request.InsuredAmount,
            Status = "InAnalysis",
            CreatedAt = now,
            UpdatedAt = now
        };

        await _repo.CreateAsync(proposal, ct);

        return CreatedAtAction(nameof(GetById), new { id = proposal.Id }, proposal);
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<ProposalDto>>> List(CancellationToken ct)
    {
        var items = await _repo.ListAsync(ct);
        return Ok(items);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<ProposalDto>> GetById(Guid id, CancellationToken ct)
    {
        var proposal = await _repo.GetByIdAsync(id, ct);
        if (proposal is null) return NotFound();

        return Ok(proposal);
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateProposalRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.CustomerName))
            return BadRequest("CustomerName is required.");

        if (request.InsuredAmount <= 0)
            return BadRequest("InsuredAmount must be greater than 0.");

        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        existing.CustomerName = request.CustomerName.Trim();
        existing.InsuredAmount = request.InsuredAmount;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        await _repo.UpdateAsync(existing, ct);
        return NoContent();
    }

    [HttpPatch("{id:guid}/status")]
    public async Task<IActionResult> ChangeStatus(
        Guid id,
        [FromBody] UpdateProposalStatusRequest request,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(request.Status))
            return BadRequest("Status is required.");

        if (!IsValidStatus(request.Status))
            return BadRequest("Invalid status. Use: InAnalysis, Approved, Rejected.");

        var existing = await _repo.GetByIdAsync(id, ct);
        if (existing is null) return NotFound();

        if (!string.Equals(existing.Status, "InAnalysis", StringComparison.OrdinalIgnoreCase))
            return Conflict($"Cannot change status from {existing.Status}.");

        var normalized = NormalizeStatus(request.Status);

        if (string.Equals(normalized, "InAnalysis", StringComparison.OrdinalIgnoreCase))
            return Conflict("Cannot set status back to InAnalysis.");

        existing.Status = normalized;
        existing.UpdatedAt = DateTimeOffset.UtcNow;

        await _repo.UpdateAsync(existing, ct);
        return NoContent();
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken ct)
    {
        var deleted = await _repo.DeleteAsync(id, ct);
        if (!deleted) return NotFound();

        return NoContent();
    }

    private static bool IsValidStatus(string status)
        => status.Trim().Equals("InAnalysis", StringComparison.OrdinalIgnoreCase)
           || status.Trim().Equals("Approved", StringComparison.OrdinalIgnoreCase)
           || status.Trim().Equals("Rejected", StringComparison.OrdinalIgnoreCase);

    private static string NormalizeStatus(string status)
    {
        var s = status.Trim();
        if (s.Equals("InAnalysis", StringComparison.OrdinalIgnoreCase)) return "InAnalysis";
        if (s.Equals("Approved", StringComparison.OrdinalIgnoreCase)) return "Approved";
        return "Rejected";
    }
}