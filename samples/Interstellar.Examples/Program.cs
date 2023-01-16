using System.Reflection;
using Interstellar;
using Interstellar.EventStorage.InMemory;
using Interstellar.Examples;
using Interstellar.MediatR;
using MediatR;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static Task Main(string[] args) => Build().RunAsync();

    private static ExampleRunner Build()
    {
        var services = new ServiceCollection();

        ServiceProvider provider = services
            .AddInterstellar(configuration =>
            {
                Thing.Configure(configuration);
                ThingWotsits.Configure(configuration);
            })
            .AddInterstellarInMemoryEventStorage()
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddSingleton<IEventDeliverer, MediatREventDeliverer>()
            .AddSingleton<UserService>()
            .AddSingleton<ExampleRunner>()
            .BuildServiceProvider();

        return provider.GetRequiredService<ExampleRunner>();
    }
}