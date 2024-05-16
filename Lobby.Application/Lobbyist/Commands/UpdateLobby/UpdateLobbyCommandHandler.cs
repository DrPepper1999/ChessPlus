using Lobby.Application.Common.Interfaces;
using MediatR;

namespace Lobby.Application.Lobbyist.Commands.UpdateLobby;

public class UpdateLobbyCommandHandler(ILobbyRepository _lobbyRepository) : IRequestHandler<UpdateLobbyCommand, Unit>
{
    public async Task<Unit> Handle(UpdateLobbyCommand request, CancellationToken cancellationToken)
    {
        var lobby = await _lobbyRepository.GetAsync(request.LobbyId);

        if (lobby is null)
        {
            throw new Exception();
        }

        lobby.Name = "new Name";

        await _lobbyRepository.UpdateAsync(request.LobbyId, lobby);

        return Unit.Value;
    }
}