using GameChess.Application.Common.Interfaces;
using GameChess.Infrastructure.Games.Persistence;
using GameChess.Infrastructure.Interceptors;
using GameChess.Infrastructure.LobbyService;
using GameChess.Infrastructure.Marten;
using Marten;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Weasel.Core;

namespace GameChess.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddHttpContextAccessor()
            .AddPersistence(configuration)
            .AddHttpClients()
            .AddServices();
        
        return services;
    }
    
    private static IServiceCollection AddServices(this IServiceCollection services)
    {
        services.AddScoped<ILobbyService, LobbyService.LobbyService>();
        
        return services;
    }
    
    private static IServiceCollection AddHttpClients(this IServiceCollection services)
    {
        services.AddOptions<LobbySettings>()
            .BindConfiguration(LobbySettings.Section)
            .ValidateDataAnnotations()
            .ValidateOnStart();
        services.AddHttpClient("lobby",(serviceProvider, httpClient) =>
        {
            var lobbySettings = serviceProvider.GetRequiredService<IOptions<LobbySettings>>().Value;

            httpClient.BaseAddress = new Uri(lobbySettings.Url);
        });
        
        return services;
    }
    
    private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var martenSettings = new MartenSettings();
        configuration.Bind(MartenSettings.SectionName, martenSettings);

        services.AddMarten(options =>
            {
                options.Connection(martenSettings.ConnectionString);
                options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                
                options.Events.DatabaseSchemaName = martenSettings.WriteModelSchema;
                options.DatabaseSchemaName = martenSettings.ReadModelSchema;

                options.UseNewtonsoftForSerialization(
                    EnumStorage.AsString,
                    nonPublicMembersStorage: NonPublicMembersStorage.All
                );

                // options.Projections.Errors.SkipApplyErrors = false;
                // options.Projections.Errors.SkipSerializationErrors = false;
                // options.Projections.Errors.SkipUnknownEvents = false;

                if (martenSettings.UseMetadata)
                {
                    options.Events.MetadataConfig.CausationIdEnabled = true;
                    options.Events.MetadataConfig.CorrelationIdEnabled = true;
                    options.Events.MetadataConfig.HeadersEnabled = true;
                }
            })
            .UseLightweightSessions()
            .ApplyAllDatabaseChangesOnStartup()
            .AddAsyncDaemon(martenSettings.DaemonMode)
            .AddSubscriptionWithServices<PublishDomainEventsInterceptor>(ServiceLifetime.Scoped);

        services.AddScoped<IGameRepository, GameRepository>();
        
        return services;
    }
}