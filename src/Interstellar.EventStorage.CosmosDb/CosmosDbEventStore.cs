namespace Interstellar.EventStorage.CosmosDb;

using Microsoft.Azure.Cosmos;
    
public class CosmosDbEventStore : IEventStore
{
    private const string PartitionKey = "/StreamId";
    private readonly IEventSourcingCosmosContainerProvider containerProvider;
    private readonly MessageNameTypeLookup messageNameTypeLookup;
    private readonly IEventDeliverer eventDeliverer;

    public CosmosDbEventStore(
        IEventSourcingCosmosContainerProvider containerProvider,
        MessageNameTypeLookup messageNameTypeLookup,
        IEventDeliverer eventDeliverer)
    {
        this.containerProvider = containerProvider;
        this.messageNameTypeLookup = messageNameTypeLookup;
        this.eventDeliverer = eventDeliverer;
    }

    public async Task<EventStream> GetEventsAsync(string streamId)
    {
        Container container = await containerProvider.ProvideContainerAsync(PartitionKey);
            
        var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.StreamId = '{streamId}'");
        FeedIterator<EventPayloadDataItem>? queryResultSetIterator = container.GetItemQueryIterator<EventPayloadDataItem>(queryDefinition);

        var events = new List<EventPayload>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<EventPayloadDataItem>? currentResultSet = await queryResultSetIterator.ReadNextAsync();

            foreach (EventPayloadDataItem? eventPayloadDataItem in currentResultSet)
            {
                events.Add(eventPayloadDataItem.ToEventPayload(messageNameTypeLookup));
            }
        }

        return new EventStream(events);
    }

    public async Task StoreEventsAsync(EventStreamSlice toStore)
    {
        Container container = await containerProvider.ProvideContainerAsync(PartitionKey);

        var batch = container.CreateTransactionalBatch(new PartitionKey(toStore.StreamId));
        batch.WriteEventsToStream(toStore);
        batch.WriteImmediateDispatchPosition(toStore);
        await batch.ExecuteAsync();

        await DeliverEventsAsync(toStore);
        await RemoveImmediateDispatchPositionAsync(toStore, container);
    }

    private async Task DeliverEventsAsync(EventStreamSlice toStore)
    {
        foreach (EventPayload? eventPayload in toStore)
        {
            await eventDeliverer.DeliverAsync(eventPayload);
        }
    }

    private static Task<ItemResponse<ImmediateDispatchPosition>> RemoveImmediateDispatchPositionAsync(
        EventStreamSlice toStore,
        Container container)
    {
        return container.DeleteItemAsync<ImmediateDispatchPosition>(toStore.StreamId, new PartitionKey(toStore.StreamId));
    }
}