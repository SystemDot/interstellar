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