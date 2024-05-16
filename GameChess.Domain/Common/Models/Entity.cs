namespace GameChess.Domain.Common.Models;

public class Entity<TId> : IEquatable<Entity<TId>>
    where TId : notnull
{
    public TId Id { get; protected set; }
    
    private readonly Queue<IDomainEvent> _domainEvents = new();

    public List<IDomainEvent> PopDomainEvents()
    {
        var copy = _domainEvents.ToList();
        _domainEvents.Clear();

        return copy;
    }
    
    public void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Enqueue(domainEvent);
    }
    
    public Entity(TId id)
    {
        Id = id;
    }
    
    public bool Equals(Entity<TId>? other)
    {
        return Equals((object?)other);
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity<TId> entity && Id.Equals(entity.Id);
    }

    public static bool operator ==(Entity<TId> left, Entity<TId> right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(Entity<TId> left, Entity<TId> right)
    {
        return !Equals(left, right);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }
    
#pragma warning disable CS8618
    protected Entity()
    {
    }
#pragma warning restore CS8618
}
