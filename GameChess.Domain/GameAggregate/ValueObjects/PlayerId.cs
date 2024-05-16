using System.ComponentModel.DataAnnotations.Schema;

namespace GameChess.Domain.GameAggregate.ValueObjects;

public record PlayerId
{
    public Guid Value { get; } 
    
    private PlayerId(Guid value)
    {
        Value = value;
    }
    
    public static PlayerId CreateUnique()
    {
        return new PlayerId(Guid.NewGuid());
    }

    public static PlayerId Create(Guid value)
    {
        return new PlayerId(value);
    }

    private PlayerId()
    {
    }
};