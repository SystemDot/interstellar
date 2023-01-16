using System.Reflection;
using Interstellar;
using Interstellar.Examples;
using Interstellar.Examples.Messages;
using Interstellar.MediatR;
using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

public static class Program
{
    public static Task Main(string[] args)
    {
        var mediator = BuildExamples();
        mediator.Send(new CreateOrModifyThing(Guid.NewGuid(), "Thing1", "Really good thing"));
        return Task.CompletedTask;
    }

    private static IMediator BuildExamples()
    {
        var services = new ServiceCollection();

        var provider = services
            .AddMediatR(Assembly.GetExecutingAssembly())
            .AddInterstellar(configuration =>
            {
                Thing.Configure(configuration);
                ThingWotsits.Configure(configuration);
            })
            .AddInterstellarMediatR()
            .BuildServiceProvider();

        return provider.GetRequiredService<IMediator>();
    }
}


