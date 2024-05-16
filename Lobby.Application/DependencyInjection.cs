using System.Reflection;
using FluentValidation;
using Lobby.Application.Common.Behaviors;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Lobby.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddMediatR(options =>
        {
            options.RegisterServicesFromAssemblies(typeof(DependencyInjection).Assembly);

            options.AddOpenBehavior(typeof(ValidationBehavior<,>));
        });
        
        
        services.AddValidatorsFromAssemblyContaining(typeof(DependencyInjection));
        return services;
    }
}