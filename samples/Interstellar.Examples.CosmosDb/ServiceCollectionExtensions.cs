using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples.CosmosDb;

public static class ServiceCollectionExtensions
{
    const string CosmosDb = nameof(CosmosDb);
    public static IServiceCollection AddInterstellarCosmosDbExamples(this IServiceCollection services)
    {
        IConfigurationRoot config = GetConfig();
        var cosmosDbSettings = config.GetSection(CosmosDb).Get<CosmosDbSettings>();

        return services
            .AddSingleton(cosmosDbSettings!)
            .AddSingleton<CosmosDatabaseProvider>()
            .AddSingleton<ReadModelCosmosContainerProvider>();
    }

    private static IConfigurationRoot GetConfig() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
}