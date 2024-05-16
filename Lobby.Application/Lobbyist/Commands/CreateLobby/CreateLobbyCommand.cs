using MediatR;

namespace Lobby.Application.Lobbyist.Commands.CreateLobby;

public record CreateLobbyCommand(string Name, int MaxPlayers) : IRequest<Guid>;