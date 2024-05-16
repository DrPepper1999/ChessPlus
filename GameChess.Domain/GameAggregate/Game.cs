using System.Collections;
using ErrorOr;
using GameChess.Domain.Common.Models;
using GameChess.Domain.GameAggregate.Entities;
using GameChess.Domain.GameAggregate.Enums;
using GameChess.Domain.GameAggregate.ValueObjects;

namespace GameChess.Domain.GameAggregate;

public class Game : AggregateRoot<Guid>
{
    private readonly Board _board;
    public Players Players { get; private set; }

    public static Game Create(Dictionary<Color, PlayerId> playerIds)
    {
        return new Game(playerIds);
    }
    
    private Game(Dictionary<Color, PlayerId> playerIds) : base(GameId.CreateUnique().Value)
    {
        var @event = new GameCreated(playerIds);
        
        AddDomainEvent(@event);
        Apply(@event);
    }

    private void Apply(GameCreated @event)
    {
        Players = new Players(@event.Players);
    }
    
    public ErrorOr<Success> MovePiece(Coordinates from, Coordinates to)
    {
        _board.MovePiece(from, to);
        
        return Result.Success;
    }
    
    private Game() {}
}

public class Players(Dictionary<Color, PlayerId> playerIds) : IEnumerable<KeyValuePair<Color, PlayerId>>
{
    public IEnumerator<KeyValuePair<Color, PlayerId>> GetEnumerator() => playerIds.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
}

public record GameCreated(Dictionary<Color, PlayerId> Players) : IDomainEvent;
