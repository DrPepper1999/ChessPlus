namespace Lobby.Api.Models.Lobby;

public record CreateLobbyRequest(string Name, int MaxPlayers);