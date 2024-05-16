using Lobby.Application.Common.Interfaces;
using Lobby.Domain;
using MediatR;

namespace Lobby.Application.Lobbyist.Commands.CreateLobby;

public class CreateLobbyCommandHandler(ILobbyRepository _lobbyRepository) : IRequestHandler<CreateLobbyCommand, Guid>
{
    public async Task<Guid> Handle(CreateLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = new GameLobby(request.Name, request.MaxPlayers);

        await _lobbyRepository.AddAsync(lobby.Id, lobby);
        
        return lobby.Id;
    }
}