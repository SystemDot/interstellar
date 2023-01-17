using System.Reflection;
using Interstellar.EventStorage.CosmosDb;
using Interstellar.Examples.Messages;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples.CosmosDb;

public static class Program
{
    const string CosmosDb = nameof(CosmosDb);

    public static async Task Main(string[] args)
    {
        ServiceProvider serviceProvider = BuildCosmosExample();

        Console.WriteLine("Initialising");

        await serviceProvider.GetRequiredService<CosmosDatabaseProvider>().InitialiseAsync();
        while (true)
        {
            Console.WriteLine("Press 1 to run");
            Console.WriteLine("Any other key to exit");

            if (Console.ReadKey().Key != ConsoleKey.D1)
            {
                Console.WriteLine("Exiting");
                return;
            }
            
            Console.WriteLine("Running");
            
            await serviceProvider.GetRequiredService<ExampleRunner>().RunAsync();
        }
    }

    private static ServiceProvider BuildCosmosExample()
    {
        IConfigurationRoot config = GetConfig();
        var cosmosDbSettings = config.GetSection(CosmosDb).Get<CosmosDbSettings>();

        return new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddSingleton(cosmosDbSettings!)
            .AddSingleton<CosmosDatabaseProvider>()
            .AddInterstellar(
                configuration =>
                {
                    Thing.Configure(configuration);
                    ThingWotsits.Configure(configuration);
                },
                UseMessageTypes
                    .ThatImplement<IEvent>()
                    .FromAssemblyContaining<CreateOrModifyThing>()
                    .Build())
            .AddInterstellarCosmosDbEventStorage<EventSourcingCosmosContainerProvider>()
            .AddInterstellarExamples()
            .BuildServiceProvider();
    }

    private static IConfigurationRoot GetConfig() =>
        new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
}