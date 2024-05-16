using GameChess.Domain.GameAggregate.Entities;
using GameChess.Domain.GameAggregate.ValueObjects;

namespace GameChess.Domain.GameAggregate.Interfaces;

public interface IMoveBehavior
{
    IEnumerable<CoordinatesShift> GetMoves(Board board, Coordinates currentCoordinates);
}