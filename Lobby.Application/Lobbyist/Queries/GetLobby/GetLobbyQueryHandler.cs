using ErrorOr;
using Lobby.Application.Common.Interfaces;
using Lobby.Domain;
using MediatR;

namespace Lobby.Application.Lobbyist.Queries.GetLobby;

public class GetLobbyQueryHandler(ILobbyRepository _lobbyRepository) : IRequestHandler<GetLobbyQuery, ErrorOr<GameLobby>>
{
    public async Task<ErrorOr<GameLobby>> Handle(GetLobbyQuery request, CancellationToken cancellationToken)
    {
        var lobby = await _lobbyRepository.GetAsync(request.Id);

        if (lobby is null)
        {
            return Error.NotFound("Lobby.NotFound", "Lobby with given ID does not exist");
        }

        return lobby;
    }
}