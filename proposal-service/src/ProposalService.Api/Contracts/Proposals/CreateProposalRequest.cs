namespace ProposalService.Api.Contracts.Proposals;

public sealed class CreateProposalRequest
{
    public string CustomerName { get; set; } = default!;
    public decimal InsuredAmount { get; set; }
}