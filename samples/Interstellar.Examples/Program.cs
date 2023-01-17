using System.Reflection;
using Interstellar.EventStorage.CosmosDb;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples;

using Interstellar.EventStorage.InMemory;
using Interstellar.Examples.Cosmos;
using Interstellar.Examples.Messages;

public static class Program
{
    const string CosmosDb = nameof(CosmosDb);

    public static async Task Main(string[] args)
    {
        Console.WriteLine("1.In memory example");
        Console.WriteLine("2.CosmosDb example");
        Console.WriteLine("Any other key exits");

        ConsoleKey key = Console.ReadKey().Key;

        if (key == ConsoleKey.D1)
        {
            ExampleRunner exampleRunner = BuildInMemoryExample();
            await exampleRunner.RunAsync();
        }

        if (key == ConsoleKey.D2)
        {
            ExampleRunner exampleRunner = await BuildCosmosExampleAsync();
            await exampleRunner.RunAsync();
        }
    }

    private static ExampleRunner BuildInMemoryExample()
    {
        ServiceProvider provider = new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar(
                configuration =>
                {
                    Thing.Configure(configuration);
                    ThingWotsits.Configure(configuration);
                },
                MessageNameTypeLookup.FromTypesFromAssemblyContainingAndImplements<CreateOrModifyThing, IEvent>())
            .AddInterstellarInMemoryEventStorage()
            .AddInterstellarExamples()
            .BuildServiceProvider();

        return provider.GetRequiredService<ExampleRunner>();
    }

    private static async Task<ExampleRunner> BuildCosmosExampleAsync()
    {
        IConfigurationRoot config = GetConfig();
        var cosmosDbSettings = config.GetSection(CosmosDb).Get<CosmosDbSettings>();
        
        ServiceProvider provider = new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddSingleton(cosmosDbSettings!)
            .AddSingleton<CosmosDatabaseProvider>()
            .AddInterstellar(
                configuration =>
                {
                    Thing.Configure(configuration);
                    ThingWotsits.Configure(configuration);
                },
                MessageNameTypeLookup.FromTypesFromAssemblyContainingAndImplements<CreateOrModifyThing, IEvent>())
            .AddInterstellarCosmosDbEventStorage<EventSourcingCosmosContainerProvider>()
            .AddInterstellarExamples()
            .BuildServiceProvider();

        await provider.GetRequiredService<CosmosDatabaseProvider>().InitialiseAsync();

        return provider.GetRequiredService<ExampleRunner>();
    }

    private static IConfigurationRoot GetConfig() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
}