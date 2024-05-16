using MediatR.NotificationPublishers;
using Microsoft.Extensions.DependencyInjection;

namespace GameChess.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection service)
    {
        service
            .AddMediator();

        return service;
    }
    
    private static IServiceCollection AddMediator(this IServiceCollection services)
    {
        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssemblyContaining(typeof(DependencyInjection));

            config.NotificationPublisher = new ForeachAwaitPublisher();
        });

        return services;
    }
}