using MediatR;
using Microsoft.AspNetCore.Http;

namespace Lobby.Application.Lobbyist.Commands.UpdateLobby;

public record UpdateLobbyCommand(Guid LobbyId) : IRequest<Unit>;