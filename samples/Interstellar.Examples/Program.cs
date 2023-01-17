using System.Reflection;
using Interstellar.EventStorage.CosmosDb;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples;

public static class Program
{
    const string CosmosDb = nameof(CosmosDb);

    public static Task Main(string[] args) => Build().RunAsync();

    private static ExampleRunner Build()
    {
        var config = GetConfig();
        var cosmosDbSettings = config.GetSection(CosmosDb).Get<CosmosDbSettings>();
        
        ServiceProvider provider = new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar(configuration =>
            {
                Thing.Configure(configuration);
                ThingWotsits.Configure(configuration);
            })
            .AddInterstellarCosmosDbEventStorage<EventSourcingCosmosContainerProvider>()
            .AddInterstellarExamples(cosmosDbSettings!)
            .BuildServiceProvider();

        return provider.GetRequiredService<ExampleRunner>();
    }

    private static IConfigurationRoot GetConfig()
    {
        return new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
    }
}