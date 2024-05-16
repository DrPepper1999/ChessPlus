using ErrorOr;
using Lobby.Domain;
using MediatR;

namespace Lobby.Application.Lobbyist.Queries.GetLobby;

public record GetLobbyQuery(Guid Id) : IRequest<ErrorOr<GameLobby>>;