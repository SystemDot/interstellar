namespace Interstellar.Tests.Unit;

using Interstellar.EventStorage.InMemory;
using Microsoft.Extensions.DependencyInjection;

public static class ServicesBuilder
{
    public static ServiceProvider BuildServices()
    {
        return new ServiceCollection()
            .AddInterstellar<TestEventDeliverer>(
                TestAggregate.Configure,
                UseMessageTypes
                    .ThatImplement<IEvent>()
                    .FromAssemblyContaining<TestStateOneEvent>()
                    .Build())
            .AddInterstellarInMemoryEventStorage()
            .AddInterstellarTests()
            .BuildServiceProvider();
    }
}