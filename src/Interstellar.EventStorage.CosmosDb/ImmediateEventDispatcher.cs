using Microsoft.Azure.Cosmos;

namespace Interstellar.EventStorage.CosmosDb;

public class ImmediateEventDispatcher
{
    private readonly IEventSourcingCosmosContainerProvider containerProvider;
    private readonly IEventDeliverer eventDeliverer;

    public ImmediateEventDispatcher(IEventSourcingCosmosContainerProvider containerProvider, IEventDeliverer eventDeliverer)
    {
        this.containerProvider = containerProvider;
        this.eventDeliverer = eventDeliverer;
    }

    public async Task DispatchEventsAsync(EventStreamSlice toStore)
    {
        await eventDeliverer.DeliverEventsAsync(toStore);
        await RemoveAsync(toStore.ToImmediateUpdateEventIndexId(), toStore.StreamId);
    }
    
    private async Task RemoveAsync(string id, string streamId)
    {
        Container container = await GetContainerAsync();
        await container.DeleteItemAsync<ImmediateUpdateEventIndex>(id, new PartitionKey(streamId));
    }
    
    private Task<Container> GetContainerAsync() =>
        containerProvider.ProvideContainerAsync(PartitionKeys.StreamId);
}