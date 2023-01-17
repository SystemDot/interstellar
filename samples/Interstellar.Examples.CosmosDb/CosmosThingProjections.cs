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
    private readonly CosmosDatabaseProvider databaseProvider;

    public CosmosThingProjections(CosmosDatabaseProvider databaseProvider)
    {
        this.databaseProvider = databaseProvider;
    }

    public async Task Handle(ThingCreated notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingCreated));

        var container = await new CosmosContainerProvider(databaseProvider, "ThingList")
            .ProvideContainerAsync("/Type");

        await container.UpsertItemAsync(
            new ThingListItem
            {
                Id = notification.Id,
                Type = 1,
                Name = notification.Name,
            }, 
            cancellationToken: cancellationToken);
    }

    public async Task Handle(ThingModified notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(ThingModified));

        var container = await new CosmosContainerProvider(databaseProvider, "ThingList")
            .ProvideContainerAsync("/Type");

        await container.UpsertItemAsync(
            new ThingListItem
            {
                Id = notification.Id,
                Type = 1,
                Name = notification.Name,
            },
            cancellationToken: cancellationToken);
    }

    public async Task Handle(WotsitStarted notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitStarted));

        var container = await new CosmosContainerProvider(databaseProvider, "WotsitActivity")
            .ProvideContainerAsync("/ThingId");

        await container.UpsertItemAsync(
            new WotsitActivity
            {
                Id = Guid.NewGuid(),
                ThingId = notification.ThingId,
                State = "Started",
            },
            cancellationToken: cancellationToken);
    }

    public async Task Handle(WotsitMade notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitMade));

        var container = await new CosmosContainerProvider(databaseProvider, "WotsitActivity")
            .ProvideContainerAsync("/ThingId");

        await container.UpsertItemAsync(
            new WotsitActivity
            {
                Id = Guid.NewGuid(),
                ThingId = notification.ThingId,
                State = "Made",
            },
            cancellationToken: cancellationToken);
    }

    public async Task Handle(WotsitDestroyed notification, CancellationToken cancellationToken)
    {
        Console.WriteLine(nameof(WotsitDestroyed));

        var container = await new CosmosContainerProvider(databaseProvider, "WotsitActivity")
            .ProvideContainerAsync("/ThingId");

        await container.UpsertItemAsync(
            new WotsitActivity
            {
                Id = Guid.NewGuid(),
                ThingId = notification.ThingId,
                State = "Destroyed",
            },
            cancellationToken: cancellationToken);
    }
}