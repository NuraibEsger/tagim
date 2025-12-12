using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Tagim.Api.Middleware;
using Tagim.Api.Services;
using Tagim.Application;
using Tagim.Application.Interfaces;
using Tagim.Infrastructure;
using Tagim.Infrastructure.Persistence;

namespace Tagim.Api;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddApplicationServices();

        builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                var jwtSettings = builder.Configuration.GetSection("JwtSettings");
                var secretKey = jwtSettings["Secret"];

                var key = Encoding.UTF8.GetBytes(secretKey!);

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtSettings["Issuer"],
                    ValidAudience = jwtSettings["Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ClockSkew = TimeSpan.Zero
                };
            });
        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();
        
        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowAll", policy =>
            {
                policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });

        builder.Services.AddScoped<ApplicationDbContext>();
        
        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }
    
        app.UseHttpsRedirection();
        
        app.UseExceptionHandler();
        
        app.UseRouting();
        
        app.UseCors("AllowAll");
        
        app.UseAuthorization();
        app.UseAuthorization();

        app.MapControllers();

        await using (var scope = app.Services.CreateAsyncScope())
        {
            var initialiser = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitialiser>();
            await initialiser.InitializeAsync();
            await initialiser.SeedAsync();
        }
        
        await app.RunAsync();
    }
}