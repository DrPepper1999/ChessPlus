using GameChess.Domain.GameAggregate.Enums;

namespace GameChess.Application.Models;

public record Lobby(Guid Id, string Name, Dictionary<Color, Guid> Players, LobbyStatus Status, int MaxPlayers, int Point);

public enum LobbyStatus
{
    
}