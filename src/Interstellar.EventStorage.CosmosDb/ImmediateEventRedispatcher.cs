using Microsoft.Azure.Cosmos;

namespace Interstellar.EventStorage.CosmosDb;

public class ImmediateEventRedispatcher
{
    private readonly IEventSourcingCosmosContainerProvider containerProvider;
    private readonly ImmediateEventDispatcher eventDeliverer;
    private readonly IEventStore eventStore;

    public ImmediateEventRedispatcher(IEventSourcingCosmosContainerProvider containerProvider, ImmediateEventDispatcher eventDeliverer, IEventStore eventStore)
    {
        this.containerProvider = containerProvider;
        this.eventDeliverer = eventDeliverer;
        this.eventStore = eventStore;
    }

    public async Task RedispatchUnDispatchedEventsAsync()
    {
        Container container = await GetContainerAsync();
        var queryResultSetIterator = GetQueryResultSetIterator(container);

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<ImmediateUpdateEventIndex>? currentResultSet = await queryResultSetIterator.ReadNextAsync();

            foreach (ImmediateUpdateEventIndex? index in currentResultSet)
            {
                EventStream events = await eventStore.GetEventsAsync(index.StreamId);
                var slice = new EventStreamSlice(index.StreamId, index.Index, events.SliceAt(index.Index));
                await eventDeliverer
                    .DispatchEventsAsync(slice);
            }
        }
    }

    private FeedIterator<ImmediateUpdateEventIndex> GetQueryResultSetIterator(Container container)
    {
        var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.Type = '{nameof(ImmediateUpdateEventIndex)}'");
        return container.GetItemQueryIterator<ImmediateUpdateEventIndex>(queryDefinition);
    }

    private Task<Container> GetContainerAsync() =>
        containerProvider.ProvideContainerAsync(PartitionKeys.StreamId);
}