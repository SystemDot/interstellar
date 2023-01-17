using Microsoft.Azure.Cosmos;

namespace Interstellar.EventStorage.CosmosDb;

public interface IEventSourcingCosmosContainerProvider
{
    Task<Container> ProvideContainerAsync(string partitionKey);
}