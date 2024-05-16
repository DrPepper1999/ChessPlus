using Lobby.Application.Common.Interfaces;
using Lobby.infrastructure.Cache;
using Lobby.infrastructure.Persistence.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Lobby.infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddServices()
            .AddPersistence(configuration);
        
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddSingleton<ICacheClient, CacheClient>()
            .AddScoped<ILobbyRepository, LobbyRepository>();
        
        return services;
    }
    
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            var connection = configuration.GetConnectionString("Redis");
            options.Configuration = connection;
        });
        
        // services.AddScoped<IRemindersRepository, RemindersRepository>();
        // services.AddScoped<IUsersRepository, UsersRepository>();

        return services;
    }
}