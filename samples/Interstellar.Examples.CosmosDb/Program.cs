using System.Reflection;
using Interstellar.EventStorage.CosmosDb;
using Interstellar.Examples.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples.CosmosDb;

public static class Program
{

    public static async Task Main(string[] args)
    {
        ServiceProvider serviceProvider = BuildCosmosExample();

        Console.WriteLine("Initialising");

        await serviceProvider.GetRequiredService<CosmosDatabaseProvider>().InitialiseAsync();
        await serviceProvider.GetRequiredService<CosmosDatabaseInitialiser>().InitialiseAsync();
        while (true)
        {
            Console.WriteLine("Press 1 to run");
            Console.WriteLine("Press 2 to re-run all events through projections");
            Console.WriteLine("Any other key to exit");
            
            var exampleRunner = serviceProvider.GetRequiredService<ExampleRunner>();

            var consoleKey = Console.ReadKey().Key;

            if (consoleKey == ConsoleKey.D1)
            {
                Console.WriteLine("Running");
                await exampleRunner.RunAsync();
            }
            else if (consoleKey == ConsoleKey.D2)
            {
                Console.WriteLine("Re-running all events through projections");
                await exampleRunner.ReRunAsync();
            }
            else
            {
                Console.WriteLine("Exiting");
                return;
            }

        }
    }

    private static ServiceProvider BuildCosmosExample()
    {
        return new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar<MediatREventDeliverer>(
                configuration =>
                {
                    Thing.Configure(configuration);
                    ThingWotsits.Configure(configuration);
                },
                UseMessageTypes
                    .ThatImplement<INotification>()
                    .FromAssemblyContaining<CreateOrModifyThing>()
                    .Build())
            .AddInterstellarCosmosDbEventStorage<EventSourcingCosmosContainerProvider>(
                new CosmosDbEventStoreSettings())
            .AddInterstellarExamples()
            .AddInterstellarCosmosDbExamples()
            .BuildServiceProvider();
    }
}