using Interstellar.EventStorage.CosmosDb;
using Microsoft.Azure.Cosmos;

namespace Interstellar.Tests.Performance;

public class EventSourcingCosmosContainerProvider : IEventSourcingCosmosContainerProvider
{
    private readonly CosmosContainerProvider inner;

    public EventSourcingCosmosContainerProvider(CosmosDatabaseProvider databaseProvider)
    {
        inner = new CosmosContainerProvider(databaseProvider, "EventStore");
    }

    public Task<Container> ProvideContainerAsync(string partitionKey) =>
        inner.ProvideContainerAsync(partitionKey);
}