using GameChess.Application.Models;

namespace GameChess.Application.Common.Interfaces;

public interface ILobbyService
{
    public Task<Lobby> GetLobbyAsync(Guid lobbyId);
}