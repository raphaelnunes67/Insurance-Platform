using System.Collections.Concurrent;
using ProposalService.Api.Contracts.Proposals;

namespace ProposalService.Api.Storage;

public sealed class InMemoryProposalRepository : IProposalRepository
{
    private readonly ConcurrentDictionary<Guid, ProposalDto> _db = new();

    public Task<ProposalDto> CreateAsync(ProposalDto proposal, CancellationToken ct)
    {
        _db[proposal.Id] = proposal;
        return Task.FromResult(proposal);
    }

    public Task<IReadOnlyList<ProposalDto>> ListAsync(CancellationToken ct)
    {
        IReadOnlyList<ProposalDto> items = _db.Values
            .OrderByDescending(x => x.CreatedAt)
            .ToList();

        return Task.FromResult(items);
    }

    public Task<ProposalDto?> GetByIdAsync(Guid id, CancellationToken ct)
    {
        _db.TryGetValue(id, out var proposal);
        return Task.FromResult(proposal);
    }

    public Task<bool> UpdateAsync(ProposalDto proposal, CancellationToken ct)
    {
        if (!_db.ContainsKey(proposal.Id))
            return Task.FromResult(false);

        _db[proposal.Id] = proposal;
        return Task.FromResult(true);
    }

    public Task<bool> DeleteAsync(Guid id, CancellationToken ct)
        => Task.FromResult(_db.TryRemove(id, out _));
}