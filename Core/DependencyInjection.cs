using Core.Commands;
using Core.Events;
using Core.Ids;
using Core.OpenTelemetry;
using Core.Queries;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Core;

public static class DependencyInjection
{
    public static IServiceCollection AddCoreServices(this IServiceCollection services)
    {
        services
            .AddSingleton(ActivityScope.Instance)
            .AddEventBus()
            .AddInMemoryCommandBus()
            .AddQueryBus();
        
        services.TryAddScoped<IIdGenerator, NulloIdGenerator>();
        services.TryAddSingleton(EventTypeMapper.Instance);

        return services;
    }
}
