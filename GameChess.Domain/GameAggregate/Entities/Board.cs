using System.Collections;
using ErrorOr;
using GameChess.Domain.GameAggregate.Enums;
using GameChess.Domain.GameAggregate.ValueObjects;

namespace GameChess.Domain.GameAggregate.Entities;

public class Board : IEnumerable<Piece>
{
    /// <summary>
    /// coordinates -> piece.
    /// </summary>
    private readonly Dictionary<Coordinates, Piece> _pieces = new();

    public Size Size => new Size(8, 8);
    
    public Piece? this[Coordinates coordinates] => _pieces.GetValueOrDefault(coordinates);

    private Board(Dictionary<Coordinates, Piece> pieces)
    {
        _pieces = pieces;
    }

    public static ErrorOr<Board> Create(IEnumerable<Piece> pieces)
    {
        try
        {
            var piecesDic = pieces.ToDictionary(key => key.Coordinates);
            return new Board(piecesDic);
        }
        catch (Exception e)
        {
            return Error.Failure("Border.NotUniqueCoordinates", e.Message); 
        }
    }
    
    public ErrorOr<Success> MovePiece(Coordinates from, Coordinates to)
    {
        var piece = this[from];

        if (piece is null)
        { 
            return Error.NotFound("Border.SquareIsEmpty", "Square is empty");
        }

        _pieces.Remove(from);

        piece.Coordinates = to;
        
        SetPiece(piece);

        return Result.Success;
    }
    
    public bool IsSquareEmpty(Coordinates coordinates) =>
        !_pieces.ContainsKey(coordinates);

    public static Color GetSquareColor(Coordinates coordinates) =>
        ((((int)coordinates.File + 1) + coordinates.Rank) % 2) == 0
            ? Color.Black
            : Color.White;
    
    private ErrorOr<Success> SetPiece(Piece piece)
    {
        if (!_pieces.TryAdd(piece.Coordinates, piece))
        {
            _pieces[piece.Coordinates] = piece;
        }
        
        
        return Result.Success;
    }
    
    public IEnumerator<Piece> GetEnumerator() => _pieces.Values.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }
    
    private Board() { }
}