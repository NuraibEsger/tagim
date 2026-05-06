using Microsoft.Extensions.Configuration;
using Serilog;
using Serilog.Sinks.Elasticsearch;

namespace Tagim.Infrastructure.Extensions;

public static class LoggingExtensions
{
    public static LoggerConfiguration ConfigureElasticsearch(
        this LoggerConfiguration loggerConfig,
        IConfiguration configuration,
        string environment)
    {
        var uri = configuration["Elasticsearch:Uri"]!;
        var indexFormat = string.Format(
            configuration["Elasticsearch:IndexFormat"]!,
            DateTime.UtcNow);

        return loggerConfig
            .Enrich.FromLogContext()
            .Enrich.WithMachineName()
            .Enrich.WithEnvironmentName()
            .Enrich.WithProperty("Application", "Tagim")
            .Enrich.WithProperty("Environment", environment)
            .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(uri))
            {
                IndexFormat = indexFormat,
                AutoRegisterTemplate = true,
                AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv8,
                NumberOfShards = 2,
                NumberOfReplicas = 1,
                FailureCallback = (e, ex) =>
                    Console.WriteLine($"Serilog ES sink failure: {ex?.Message}"),
                EmitEventFailure =
                    EmitEventFailureHandling.WriteToSelfLog |
                    EmitEventFailureHandling.RaiseCallback
            });

    }
}