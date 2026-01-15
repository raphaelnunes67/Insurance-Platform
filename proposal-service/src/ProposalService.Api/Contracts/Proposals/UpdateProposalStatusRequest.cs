namespace ProposalService.Api.Contracts.Proposals;

public sealed class UpdateProposalStatusRequest
{
    public string Status { get; set; } = default!;
}