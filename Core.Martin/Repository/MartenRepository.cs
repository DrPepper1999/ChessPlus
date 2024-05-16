using Core.Aggregates;
using Marten;

namespace Core.Martin.Repository;

public interface IMartenRepository<T> where T : class, IAggregate
{
    Task<T?> Find(Guid id, CancellationToken cancellationToken);
    Task<long> Add(T aggregate, CancellationToken cancellationToken = default);
    Task<long> Update(T aggregate, long? expectedVersion = null, CancellationToken cancellationToken = default);
    Task<long> Delete(T aggregate, long? expectedVersion = null, CancellationToken cancellationToken = default);
}

public class MartenRepository<T>(IDocumentSession documentSession): IMartenRepository<T>
    where T : class, IAggregate
{
    public Task<T?> Find(Guid id, CancellationToken ct) =>
        documentSession.Events.AggregateStreamAsync<T>(id, token: ct);

    public async Task<long> Add(T aggregate, CancellationToken ct = default)
    {
        var events = aggregate.PopDomainEvents();

        documentSession.Events.StartStream<IAggregate>( // TODO Aggregate 
            aggregate.Id,
            events
        );

        await documentSession.SaveChangesAsync(ct).ConfigureAwait(false);

        return events.Count;
    }

    public async Task<long> Update(T aggregate, long? expectedVersion = null, CancellationToken ct = default)
    {
        var events = aggregate.PopDomainEvents();

        var nextVersion = (expectedVersion ?? aggregate.Version) + events.Count;

        documentSession.Events.Append(
            aggregate.Id,
            nextVersion,
            events
        );

        await documentSession.SaveChangesAsync(ct).ConfigureAwait(false);

        return nextVersion;
    }

    public Task<long> Delete(T aggregate, long? expectedVersion = null, CancellationToken ct = default) =>
        Update(aggregate, expectedVersion, ct);
}
