using System.Text.Json;
using Lobby.Application.Common.Interfaces;
using Lobby.Domain;
using Lobby.infrastructure.Cache;
using StackExchange.Redis;

namespace Lobby.infrastructure.Persistence.Repositories;

// TODO возможно сдлеать общий cacheRepository с internel методами или запихнуть эти методы в CacheClient с этими методами
public class LobbyRepository(ICacheClient _cacheClient) : ILobbyRepository
{
    private const string Key = nameof(GameLobby);

    public async Task<GameLobby?> GetAsync(Guid id)
    {
        var db = _cacheClient.Database;
        var cachedValue = await db.HashGetAsync(Key, id.ToString());

        if (cachedValue.IsNullOrEmpty)
        {
            return null;
        }

        var value = JsonSerializer.Deserialize<GameLobby>(cachedValue!);

        return value;
    }
    
    public async Task<IEnumerable<GameLobby>> GetAllAsync()
    {
        var db = _cacheClient.Database;

        var completeSet = await db.HashGetAllAsync(Key);

        if (completeSet.Length <= 0)
        {
            return Array.Empty<GameLobby>();
        }
        
        var result = Array.ConvertAll(completeSet, entry => 
            JsonSerializer.Deserialize<GameLobby>(entry.Value, new JsonSerializerOptions()
            {
                IncludeFields = true,
            })).ToList();
        
        return result;
    }

    public async IAsyncEnumerable<GameLobby> GetAllAsync(int pageNumber, int pageSize)
    {
        var db = _cacheClient.Database;

        var completeSet = db.HashScanAsync(Key, pageSize: pageSize, pageOffset: pageNumber * pageSize);
        
        var index = 0;
        await foreach (var entry in completeSet)
        {
            yield return JsonSerializer.Deserialize<GameLobby>(entry.Value, new JsonSerializerOptions()
            {
                IncludeFields = true,
            });

            if (index++ >= pageSize - 1)
            {
                yield break;
            }
        }
    }

    public Task<long> GetTotalCount() => _cacheClient.Database.HashLengthAsync(Key);
    
    public async Task AddAsync(Guid id, GameLobby value)
    {
        var db = _cacheClient.Database;

        if (value is null)
        {
            throw new AggregateException($"{nameof(value)} is null");
        }
           
        var cacheValue = JsonSerializer.Serialize(value);
        
        await db.HashSetAsync(Key, [new HashEntry(id.ToString(), cacheValue)]);
    }

    public async Task UpdateAsync(Guid id, GameLobby value) => await AddAsync(id, value);
    
    public async Task RemoveAsync(Guid id)
    {
        var db = _cacheClient.Database;

        await db.HashDeleteAsync(Key, id.ToString());
    }
}