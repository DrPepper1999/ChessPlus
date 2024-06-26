using Core.Configuration;
using Core.Ids;
using Core.Martin.Commands;
using Core.Martin.Ids;
using Core.Martin.Subscriptions;
using Marten;
using Marten.Events.Daemon.Resiliency;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Weasel.Core;

namespace Core.Martin;

public class MartenConfig
{
    private const string DefaultSchema = "public";

    public string ConnectionString { get; set; } = default!;

    public string WriteModelSchema { get; set; } = DefaultSchema;
    public string ReadModelSchema { get; set; } = DefaultSchema;

    public bool ShouldRecreateDatabase { get; set; } = false;

    public DaemonMode DaemonMode { get; set; } = DaemonMode.Solo;

    public bool UseMetadata = true;
}

public static class MartenConfigExtensions
{
    private const string DefaultConfigKey = "EventStore";

    public static IServiceCollection AddMarten(
        this IServiceCollection services,
        IConfiguration configuration,
        Action<StoreOptions>? configureOptions = null,
        string configKey = DefaultConfigKey,
        bool useExternalBus = false
    ) =>
        services.AddMarten(
            configuration.GetRequiredConfig<MartenConfig>(configKey),
            configureOptions,
            useExternalBus
        );

    public static IServiceCollection AddMarten(
        this IServiceCollection services,
        MartenConfig martenConfig,
        Action<StoreOptions>? configureOptions = null,
        bool useExternalBus = false
    )
    {
        services
            .AddScoped<IIdGenerator, MartenIdGenerator>()
            .AddMarten(options => SetStoreOptions(options, martenConfig, configureOptions))
            .UseLightweightSessions()
            .ApplyAllDatabaseChangesOnStartup()
            //.OptimizeArtifactWorkflow()
            .AddAsyncDaemon(martenConfig.DaemonMode)
            .AddSubscriptionWithServices<MartenEventPublisher>(ServiceLifetime.Scoped);

        if (useExternalBus)
            services.AddMartenAsyncCommandBus();

        return services;
    }

    private static void SetStoreOptions(
        StoreOptions options,
        MartenConfig martenConfig,
        Action<StoreOptions>? configureOptions = null
    )
    {
        options.Connection(martenConfig.ConnectionString);
        options.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;

        var schemaName = Environment.GetEnvironmentVariable("SchemaName");
        options.Events.DatabaseSchemaName = schemaName ?? martenConfig.WriteModelSchema;
        options.DatabaseSchemaName = schemaName ?? martenConfig.ReadModelSchema;

        options.UseNewtonsoftForSerialization(
            EnumStorage.AsString,
            nonPublicMembersStorage: NonPublicMembersStorage.All
        );

        options.Projections.Errors.SkipApplyErrors = false;
        options.Projections.Errors.SkipSerializationErrors = false;
        options.Projections.Errors.SkipUnknownEvents = false;

        if (martenConfig.UseMetadata)
        {
            options.Events.MetadataConfig.CausationIdEnabled = true;
            options.Events.MetadataConfig.CorrelationIdEnabled = true;
            options.Events.MetadataConfig.HeadersEnabled = true;
        }

        configureOptions?.Invoke(options);
    }
}
