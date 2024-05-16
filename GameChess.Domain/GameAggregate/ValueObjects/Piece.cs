using GameChess.Domain.GameAggregate.Entities;
using GameChess.Domain.GameAggregate.Enums;
using GameChess.Domain.GameAggregate.Interfaces;

namespace GameChess.Domain.GameAggregate.ValueObjects;

public record Piece // Сделать шаблонный метод в конструктор принимать enum и по нему уже _moveBehaviors создавать внутри Piece
{
    private readonly IEnumerable<IMoveBehavior> _moveBehaviors;
    
    public Color Color { get; }
    public Coordinates Coordinates { get; set; }
    
    public IEnumerable<Coordinates> GetAvailableMoveSquares(Board board)
    {
        var availableCoordinates = new HashSet<Coordinates>();

        if (Coordinates is null)
        {
            throw new ArgumentException("Coordinates is null");
        }

        var pieceMoves = _moveBehaviors
            .Select(behavior => behavior.GetMoves(board, Coordinates))
            .Aggregate(new HashSet<CoordinatesShift>(), UnionCoordinates)
            .Select(shift => Coordinates.ApplyShift(shift))
            .Where(newCoordinates => !newCoordinates.IsError)
            .Select(coordinatesResult => coordinatesResult.Value);
        
        foreach (var newCoordinates in pieceMoves)
        {
            if (IsSquareAvailableForMove(newCoordinates, board))
            {
                availableCoordinates.Add(newCoordinates);
            }
        }

        return availableCoordinates;
    }
    
    private bool IsSquareAvailableForMove(Coordinates coordinates, Board board) =>
        board.IsSquareEmpty(coordinates) || board[coordinates]!.Color != Color;

    private static HashSet<CoordinatesShift> UnionCoordinates(HashSet<CoordinatesShift> coordinates,
        IEnumerable<CoordinatesShift> coordinatesShifts)
    {
        coordinates.UnionWith(coordinatesShifts);
        return coordinates;
    }
}