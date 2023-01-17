using Interstellar.EventStorage.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Interstellar.Examples;

public class EventSourcingCosmosContainerProvider : IEventSourcingCosmosContainerProvider
{
    private readonly CosmosContainerProvider inner;

    public EventSourcingCosmosContainerProvider(CosmosDbSettings settings, CosmosDatabaseProvider databaseProvider)
    {
        inner = new CosmosContainerProvider(settings, databaseProvider, "EventStore");
    }

    public Task<Container> ProvideContainerAsync() => inner.ProvideContainerAsync();
}