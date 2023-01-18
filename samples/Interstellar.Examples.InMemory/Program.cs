using System.Reflection;
using Interstellar.EventStorage.InMemory;
using Interstellar.Examples.Messages;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples.InMemory;

public static class Program
{
    public static async Task Main(string[] args)
    {
        var serviceProvider = BuildInMemoryExample();

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

    private static ServiceProvider BuildInMemoryExample()
    {
        return new ServiceCollection()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar(
                configuration =>
                {
                    Thing.Configure(configuration);
                    ThingWotsits.Configure(configuration);
                },
                UseMessageTypes
                    .ThatImplement<INotification>()
                    .FromAssemblyContaining<CreateOrModifyThing>()
                    .Build())
            .AddInterstellarInMemoryEventStorage()
            .AddInterstellarExamples()
            .BuildServiceProvider();
    }
}