namespace GameChess.Domain.GameAggregate.ValueObjects;

public record GameId
{
    public Guid Value { get; } 
    
    private GameId(Guid value)
    {
        Value = value;
    }
    
    public static GameId CreateUnique()
    {
        return new GameId(Guid.NewGuid());
    }

    public static GameId Create(Guid value)
    {
        return new GameId(value);
    }
}