using ProposalService.Api.Contracts.Proposals;

namespace ProposalService.Api.Storage;

public interface IProposalRepository
{
    Task<ProposalDto> CreateAsync(ProposalDto proposal, CancellationToken ct);
    Task<IReadOnlyList<ProposalDto>> ListAsync(CancellationToken ct);
    Task<ProposalDto?> GetByIdAsync(Guid id, CancellationToken ct);
    Task<bool> UpdateAsync(ProposalDto proposal, CancellationToken ct);
    Task<bool> DeleteAsync(Guid id, CancellationToken ct);
}