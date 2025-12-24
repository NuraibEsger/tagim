using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Tagim.Application.Interfaces;
using Tagim.Infrastructure.Persistence;
using Tagim.Infrastructure.Services;

namespace Tagim.Infrastructure;

public static class InfrastructureServiceRegistration
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddDbContext<ApplicationDbContext>(options => 
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
        
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<ApplicationDbContext>());
        
        services.AddScoped<ITokenService, JwtTokenService>();

        services.AddSingleton<ITagGeneratorService, TagGeneratorService>();
        
        services.AddTransient<IEmailService, EmailService>();
        
        services.AddScoped<IFileStorageService, LocalFileStorageService>();

        services.AddScoped<ApplicationDbContextInitialiser>();
        
        return services;
    }
}