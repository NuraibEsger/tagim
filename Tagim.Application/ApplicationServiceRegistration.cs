using System.Reflection;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Tagim.Application.Behaviors;

namespace Tagim.Application;

public static class ApplicationServiceRegistration
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();
        
        // 1. MediatR
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly!));
        
        // 2. FluentValidation
        services.AddValidatorsFromAssembly(assembly);
        
        // 4. Pipeline Behavior
        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));
            
        return services;
    }
}