using ErrorOr;

namespace GameChess.Domain.GameAggregate.ValueObjects;

public record Coordinates(Enums.File File, int Rank)
{
    public static ErrorOr<Coordinates> Create(Enums.File file, int rank)
    {
        if ((int)file is < 0 or > (int)Enums.File.H)
        {
            return Error.Validation("File.IsBoundOfRange", "File must be between A and H");
        }
        
        if (rank is <= 0 or > 8)
        {
            return Error.Validation("Rank.IsBoundOfRange", "Rank must be between 0 and 8");
        }
        
        return new Coordinates(file, rank);
    }

    public ErrorOr<Coordinates> ApplyShift(CoordinatesShift shift)
    {
        var file = File + shift.FileShift;

        if ((int)file is < 0 or > (int)Enums.File.H)
        {
            return Error.Validation("File.IsBoundOfRange", "File must be between A and H");
        }

        var rank = Rank + shift.RankShift;

        if (rank is <= 0 or > 8)
        {
            return Error.Validation("Rank.IsBoundOfRange", "Rank must be between 0 and 8");
        }

        return new Coordinates(file, rank);
    }
}