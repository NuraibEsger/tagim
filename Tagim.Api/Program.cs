using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Tagim.Api.Middleware;
using Tagim.Api.Profiles;
using Tagim.Api.Services;
using Tagim.Application;
using Tagim.Application.Interfaces;
using Tagim.Infrastructure;
using Tagim.Infrastructure.Extensions;
using Tagim.Infrastructure.Persistence;

namespace Tagim.Api;

public abstract class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .CreateBootstrapLogger();
        
        
        Log.Information("Starting up...");
        
        var builder = WebApplication.CreateBuilder(args);
        var environment = builder.Environment.EnvironmentName;
        
        builder.Host.UseSerilog((context, services, loggerConfig) =>
        {
            loggerConfig
                .ReadFrom.Configuration(context.Configuration)
                .ReadFrom.Services(services)
                .ConfigureElasticsearch(context.Configuration, environment);

            // Also write to console in Development
            if (context.HostingEnvironment.IsDevelopment())
                loggerConfig.WriteTo.Console(
                    outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u3}] {SourceContext} {Message:lj}{NewLine}{Exception}");
        });

        // Add services to the container.
        builder.Services.AddHttpContextAccessor();
        
        builder.Services.AddAuthorization();
        
        builder.Services.AddControllers();
        
        builder.Services.AddProblemDetails();
        builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
        
        // AutoMapper
        builder.Services.AddAutoMapper(typeof(VehicleProfile));
        
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
        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Components ??= new OpenApiComponents();
                document.Servers = new List<OpenApiServer>
                {
                    new OpenApiServer { Url = "http://localhost:8080", Description = "Local Development Server" }
                };
                
                return Task.CompletedTask;
            });
        });
        
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
        
        app.UseSerilogRequestLogging(options =>
        {
            options.MessageTemplate = "HTTP {RequestMethod} {RequestPath} responded {StatusCode} in {Elapsed:0.0000} ms";
            options.EnrichDiagnosticContext = (diagnosticContext, httpContext) =>
            {
                diagnosticContext.Set("RequestHost", httpContext.Request.Host.Value);
                diagnosticContext.Set("RequestScheme", httpContext.Request.Scheme);
                diagnosticContext.Set("UserAgent", httpContext.Request.Headers.UserAgent);
                diagnosticContext.Set("UserId", httpContext.User?.Identity?.Name ?? "anonymous");
            };
        });

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options.WithTitle("Tagim API")
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.HttpClient);
            });
        }
    
        
        app.UseExceptionHandler();
        
        //app.UseHttpsRedirection();
        
        app.UseStaticFiles();
        
        app.UseRouting();
        
        app.UseCors("AllowAll");
        
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllers();
        
        await using (var scope = app.Services.CreateAsyncScope())
        {
            var initializer = scope.ServiceProvider.GetRequiredService<ApplicationDbContextInitializer>();
            await initializer.InitializeAsync();
            await initializer.SeedAsync();
        }
        
        await app.RunAsync();
    }
}