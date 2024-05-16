using StackExchange.Redis;

namespace Lobby.infrastructure.Cache;

public interface ICacheClient
{
    public IConnectionMultiplexer Connection { get; }
    
    public IDatabase Database => Connection.GetDatabase();
}

public class CacheClient : ICacheClient
{
    private readonly Lazy<IConnectionMultiplexer> _connection;

    public CacheClient(string host = "localhost", int port = 6379, bool allowAdmin = false)
    {
        var configuration = new ConfigurationOptions()
        {
            EndPoints = { { host, port }, },
            AllowAdmin = allowAdmin,
            //Password = "", //to the security for the production
            ClientName = "My Redis Client",
            ReconnectRetryPolicy = new LinearRetry(5000),
            AbortOnConnectFail = false,
        };
        _connection = new Lazy<IConnectionMultiplexer>(() =>
        {
            var redis = ConnectionMultiplexer.Connect(configuration);
            //redis.ErrorMessage += _Connection_ErrorMessage;
            //redis.InternalError += _Connection_InternalError;
            //redis.ConnectionFailed += _Connection_ConnectionFailed;
            //redis.ConnectionRestored += _Connection_ConnectionRestored;
            return redis;
        });
    }

    //for the 'GetSubscriber()' and another Databases
    public IConnectionMultiplexer Connection => _connection.Value;
}