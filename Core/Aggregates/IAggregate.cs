using Core.Projections;

namespace Core.Aggregates;

public interface IAggregate: IAggregate<Guid>
{
}

public interface IAggregate<out T>: IProjection
{
    T Id { get; }
    int Version { get; }

    List<IDomainEvent> PopDomainEvents();
}

public interface IDomainEvent
{
}
