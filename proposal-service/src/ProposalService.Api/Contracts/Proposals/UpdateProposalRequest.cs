namespace ProposalService.Api.Contracts.Proposals;

public sealed class UpdateProposalRequest
{
    public string CustomerName { get; set; } = default!;
    public decimal InsuredAmount { get; set; }
}