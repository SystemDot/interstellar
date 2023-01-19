using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Tests.Performance;

public static class ServiceCollectionExtensions
{
    const string CosmosDb = nameof(CosmosDb);
    public static IServiceCollection AddInterstellarPerformanceTests(this IServiceCollection services)
    {
        IConfigurationRoot config = GetConfig();
        var cosmosDbSettings = config.GetSection(CosmosDb).Get<CosmosDbSettings>();

        return services
            .AddSingleton(cosmosDbSettings!)
            .AddSingleton<CosmosDatabaseProvider>()
            .AddSingleton<MessageBus>()
            .AddSingleton<TestRunner>();
    }

    private static IConfigurationRoot GetConfig() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
}