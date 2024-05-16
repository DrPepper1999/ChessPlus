using GameChess.Domain.GameAggregate.Enums;

namespace GameChess.Infrastructure.LobbyService;

public record LobbyResponse(Guid Id, string Name, ICollection<(Color, Guid)> Players, LobbyStatus Status, int MaxPlayers, int Point);