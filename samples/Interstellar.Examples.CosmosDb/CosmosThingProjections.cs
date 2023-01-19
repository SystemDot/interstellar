using Interstellar.Examples.Messages;
using MediatR;
using Microsoft.Azure.Cosmos;

namespace Interstellar.Examples.CosmosDb;

public class CosmosThingProjections : 
    INotificationHandler<ThingCreated>,
    INotificationHandler<ThingModified>,
    INotificationHandler<WotsitStarted>,
    INotificationHandler<WotsitMade>,
    INotificationHandler<WotsitDestroyed>
{
    private readonly ReadModelCosmosContainerProvider containerProvider;

    public CosmosThingProjections(ReadModelCosmosContainerProvider containerProvider)
    {
        this.containerProvider = containerProvider;
    }

    public async Task Handle(ThingCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingCreated));

        var container = await containerProvider.ProvideThingListContainerAsync();
        await UpsertThingListItemAsync(container, notification.Id, notification.Name, cancellationToken);
    }

    public async Task Handle(ThingModified notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingModified));

        var container = await containerProvider.ProvideThingListContainerAsync();
        await UpsertThingListItemAsync(container, notification.Id, notification.Name, cancellationToken);
    }

    public async Task Handle(WotsitStarted notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitStarted));

        var container = await containerProvider.ProvideWotsitActivityContainerAsync();
        await UpsertWotsitActivityAsync(container, notification.ThingId, "Started", cancellationToken);
    }
    
    public async Task Handle(WotsitMade notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitMade));

        var container = await containerProvider.ProvideWotsitActivityContainerAsync();
        await UpsertWotsitActivityAsync(container, notification.ThingId, "Made", cancellationToken);
    }

    public async Task Handle(WotsitDestroyed notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitDestroyed));

        var container = await containerProvider.ProvideWotsitActivityContainerAsync();
        await UpsertWotsitActivityAsync(container, notification.ThingId, "Destroyed", cancellationToken);
    }


    private static async Task UpsertWotsitActivityAsync(Container container,
        Guid thingId,
        string state,
        CancellationToken cancellationToken)
    {
        await container.UpsertItemAsync(
            new WotsitActivity
            {
                Id = Guid.NewGuid(),
                ThingId = thingId,
                State = state,
            },
            cancellationToken: cancellationToken);
    }

    private static async Task UpsertThingListItemAsync(
        Container container,
        Guid id,
        string name,
        CancellationToken cancellationToken)
    {
        await container.UpsertItemAsync(
            new ThingListItem
            {
                Id = id,
                Type = "ThingList",
                Name = name,
            },
            cancellationToken: cancellationToken);
    }
}