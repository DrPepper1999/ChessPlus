using GameChess.Application.Common.Interfaces;

namespace GameChess.Api.Models;

public class GameService(IGameRepository gameRepository, ILobbyService lobbyService)
{
    public IGameRepository GameRepository { get; } = gameRepository;
    public ILobbyService LobbyService { get; } = lobbyService;
}