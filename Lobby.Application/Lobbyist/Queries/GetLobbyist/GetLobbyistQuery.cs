using Contracts.Common;
using Contracts.Common.Pagination;
using Lobby.Domain;
using MediatR;

namespace Lobby.Application.Lobbyist.Queries.GetLobbyist;

public record GetLobbyistQuery(int PageNumber, int PageSize) : IRequest<AsyncPaginationResponse<GameLobby>>;