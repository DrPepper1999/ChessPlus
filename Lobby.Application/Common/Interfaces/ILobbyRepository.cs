using Lobby.Domain;

namespace Lobby.Application.Common.Interfaces;

public interface ILobbyRepository
{
    Task<GameLobby?> GetAsync(Guid id);

    Task<IEnumerable<GameLobby>> GetAllAsync();

    IAsyncEnumerable<GameLobby> GetAllAsync(int pageNumber, int pageSize);

    Task<long> GetTotalCount();

    Task AddAsync(Guid id, GameLobby value);

    Task UpdateAsync(Guid id, GameLobby value);
    
    Task RemoveAsync(Guid id);
}