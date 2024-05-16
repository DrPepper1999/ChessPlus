namespace Lobby.Api.Models.Lobby;

public record GetLobbyistRequest(int PageNumber = 0, int PageSize = 20);