using Core.Aggregates;
using Marten;

namespace Core.Martin.Aggregates;

/// <summary>
/// This code assumes that Aggregate:
/// - is event-driven but not fully event-sourced
/// - streams have string identifiers
/// - aggregate is versioned, so optimistic concurrency is applied
/// </summary>
public static class DocumentSessionExtensions
{
    public static Task Add<T>(
        this IDocumentSession documentSession,
        string id,
        T aggregate,
        CancellationToken ct
    ) where T : IAggregate
    {
        documentSession.Insert(aggregate);
        documentSession.Events.Append($"events-{id}", aggregate.PopDomainEvents());

        return documentSession.SaveChangesAsync(token: ct);
    }

    public static async Task GetAndUpdate<T>(
        this IDocumentSession documentSession,
        string id,
        Action<T> handle,
        CancellationToken ct
    ) where T : IAggregate
    {
        var aggregate = await documentSession.LoadAsync<T>(id, ct).ConfigureAwait(false);

        if (aggregate is null)

        documentSession.Update(aggregate);

        documentSession.Events.Append($"events-{id}", aggregate.PopDomainEvents());

        await documentSession.SaveChangesAsync(token: ct).ConfigureAwait(false);
    }
}
