using Interstellar.Examples.Messages;
using MediatR;

namespace Interstellar.Examples.CosmosDb;

public class CosmosThingProjections : 
    INotificationHandler<ThingCreated>,
    INotificationHandler<ThingModified>,
    INotificationHandler<WotsitStarted>,
    INotificationHandler<WotsitMade>,
    INotificationHandler<WotsitDestroyed>
{
    public Task Handle(ThingCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingCreated));

        return Task.CompletedTask;
    }

    public Task Handle(ThingModified notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingModified));

        return Task.CompletedTask;
    }

    public Task Handle(WotsitStarted notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitStarted));

        return Task.CompletedTask;
    }

    public Task Handle(WotsitMade notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitMade));

        return Task.CompletedTask;
    }

    public Task Handle(WotsitDestroyed notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitDestroyed));

        return Task.CompletedTask;
    }
}
