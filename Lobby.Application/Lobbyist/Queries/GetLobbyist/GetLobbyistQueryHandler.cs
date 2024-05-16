using Contracts.Common;
using Contracts.Common.Pagination;
using Lobby.Application.Common.Interfaces;
using Lobby.Domain;
using MediatR;

namespace Lobby.Application.Lobbyist.Queries.GetLobbyist;

public class GetLobbyistQueryHandler(ILobbyRepository _lobbyRepository) : IRequestHandler<GetLobbyistQuery, AsyncPaginationResponse<GameLobby>>
{
    public async Task<AsyncPaginationResponse<GameLobby>> Handle(GetLobbyistQuery request, CancellationToken cancellationToken)
    {
        var lobbies = _lobbyRepository.GetAllAsync(request.PageNumber, request.PageSize);

        var totalCount = await _lobbyRepository.GetTotalCount();
        
        return new AsyncPaginationResponse<GameLobby>(lobbies, totalCount, request.PageNumber, request.PageSize);
    }
}