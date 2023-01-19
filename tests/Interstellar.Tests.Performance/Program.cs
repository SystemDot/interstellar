using System.Reflection;
using Interstellar.EventStorage.CosmosDb;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Tests.Performance;

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
            Console.WriteLine("How many events to create?");
            
            if (!int.TryParse(Console.ReadLine(), out int eventCount))
            {
                continue;
            }

            Console.WriteLine("How many times to run?");

            if (!int.TryParse(Console.ReadLine(), out int runCount))
            {
                continue;
            }

            Console.WriteLine("Running");

            await serviceProvider.GetRequiredService<TestRunner>().RunAsync(eventCount, runCount);
        }
    }

    private static ServiceProvider BuildCosmosExample()
    {
        return new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar<MediatREventDeliverer>(
                configuration =>
                {
                    PerformanceTestAggregate.Configure(configuration);
                },
                UseMessageTypes
                    .ThatImplement<INotification>()
                    .FromAssemblyContaining<TestPerformanceEvent>()
                    .Build())
            .AddInterstellarCosmosDbEventStorage<EventSourcingCosmosContainerProvider>()
            .AddInterstellarPerformanceTests()
            .BuildServiceProvider();
    }
}