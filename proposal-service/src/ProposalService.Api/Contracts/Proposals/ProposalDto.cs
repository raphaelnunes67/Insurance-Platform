namespace ProposalService.Api.Contracts.Proposals;

public sealed class ProposalDto
{
    public Guid Id { get; set; }
    public string CustomerName { get; set; } = default!;
    public decimal InsuredAmount { get; set; }
    public string Status { get; set; } = default!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset UpdatedAt { get; set; }
}