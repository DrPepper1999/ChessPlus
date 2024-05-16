using System.Net.Http.Json;
using GameChess.Application.Common.Interfaces;
using GameChess.Application.Models;
using MapsterMapper;

namespace GameChess.Infrastructure.LobbyService;

public class LobbyService(IHttpClientFactory factory, IMapper _mapper) : ILobbyService
{
    public async Task<Lobby> GetLobbyAsync(Guid lobbyId)
    {
        var client = factory.CreateClient("lobby");
        
        var content = await client.GetFromJsonAsync<LobbyResponse>($"lobbyist/getLobby/{lobbyId}");

        var lobby = _mapper.Map<Lobby>(content!);

        return lobby;
    }
}
