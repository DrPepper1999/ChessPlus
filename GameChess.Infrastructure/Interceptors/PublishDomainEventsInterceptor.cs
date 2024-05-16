using GameChess.Domain.Common.Models;
using Marten;
using Marten.Events.Daemon;
using Marten.Events.Daemon.Internals;
using Marten.Subscriptions;
using MediatR;

namespace GameChess.Infrastructure.Interceptors;

public class PublishDomainEventsInterceptor(IPublisher _publisher) : SubscriptionBase
{
    public override async Task<IChangeListener> ProcessEventsAsync(EventRange page, ISubscriptionController controller, IDocumentOperations operations,
        CancellationToken cancellationToken)
    {
        var domainEvents = page.Events
            .Select(@event => (IDomainEvent)@event.Data)
            .ToList();

        // Publish domain events
        foreach (var domainEvent in domainEvents)
        {
            await _publisher.Publish(domainEvent, cancellationToken);
        }
        
        return NullChangeListener.Instance;
    }
}