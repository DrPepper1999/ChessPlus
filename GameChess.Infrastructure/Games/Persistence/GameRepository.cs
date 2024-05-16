using GameChess.Application.Common.Interfaces;
using GameChess.Domain.GameAggregate;
using GameChess.Domain.GameAggregate.ValueObjects;
using Marten;

namespace GameChess.Infrastructure.Games.Persistence;

public class GameRepository : IGameRepository
{
    private readonly IDocumentSession _documentSession;

    public GameRepository(IDocumentSession documentSession)
    {
        _documentSession = documentSession;
    }

    public Task<Game?> FindAsync(Guid id, CancellationToken ct) =>
        _documentSession.Events.AggregateStreamAsync<Game>(id, token: ct);
    
    public async Task<long> AddAsync(Game game, CancellationToken ct = default)
    {
        var events = game.PopDomainEvents();

        _documentSession.Events.StartStream<Game>(
            game.Id,
            events
        );

        await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);

        return events.Count;
    }
    
    public async Task<long> UpdateAsync(Game aggregate, long? expectedVersion = null, CancellationToken ct = default)
    {
        var events = aggregate.PopDomainEvents();

        var nextVersion = (expectedVersion ?? aggregate.Version) + events.Count;

        _documentSession.Events.Append(
            aggregate.Id,
            nextVersion,
            events
        );

        await _documentSession.SaveChangesAsync(ct).ConfigureAwait(false);

        return nextVersion;
    }

    public Task<long> DeleteAsync(Game aggregate, long? expectedVersion = null, CancellationToken ct = default) =>
        UpdateAsync(aggregate, expectedVersion, ct);
}