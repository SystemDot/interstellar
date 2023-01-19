namespace Interstellar.EventStorage.CosmosDb;

using Microsoft.Azure.Cosmos;

public class CosmosDbEventStore : IEventStore
{
    private static readonly int ConflictStatusCode = 409;

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
        Container container = await GetContainerAsync();
        var queryResultSetIterator = GetQueryResultSetIterator(streamId, container);
        var events = await GetEventPayloadsFromQueryIterator(queryResultSetIterator);
        return new EventStream(events);
    }

    public async Task StoreEventsAsync(EventStreamSlice toStore)
    {
        Container container = await GetContainerAsync();
        
        await WriteEventsAsync(toStore, container);
        await eventDeliverer.DeliverEventsAsync(toStore);
        await RemoveImmediateDispatchPositionAsync(toStore, container);
    }

    private static async Task WriteEventsAsync(EventStreamSlice toStore, Container container)
    {
        try
        {
            var result = await container.Scripts.ExecuteStoredProcedureAsync<int>(
                StoredProcedures.EventStorage,
                new PartitionKey(toStore.StreamId),
                new dynamic[] { toStore.ToEventStreamSliceDataItem() });

            if (result.Resource < toStore.Events.Count())
            {
                await WriteEventsAsync(toStore.RemoveFirstNumberOfEvents(result.Resource), container);
            }
        }
        catch (CosmosException e)
        {
            if (e.SubStatusCode == ConflictStatusCode)
            {
                throw new ExpectedEventIndexIncorrectException(toStore.StreamId, toStore.StartIndex);
            }
            throw;
        }
    }

    private Task<Container> GetContainerAsync() => 
        containerProvider.ProvideContainerAsync(PartitionKeys.StreamId);

    private static FeedIterator<EventPayloadDataItem> GetQueryResultSetIterator(string streamId, Container container)
    {
        var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.StreamId = '{streamId}'");
        return container.GetItemQueryIterator<EventPayloadDataItem>(queryDefinition);
    }

    private async Task<IEnumerable<EventPayload>> GetEventPayloadsFromQueryIterator(FeedIterator<EventPayloadDataItem> queryResultSetIterator)
    {
        var events = new List<EventPayload>();

        while (queryResultSetIterator.HasMoreResults)
        {
            FeedResponse<EventPayloadDataItem>? currentResultSet = await queryResultSetIterator.ReadNextAsync();

            foreach (EventPayloadDataItem? eventPayloadDataItem in currentResultSet)
            {
                if (Guid.TryParse(eventPayloadDataItem.Id, out _))
                {
                    events.Add(eventPayloadDataItem.ToEventPayload(messageNameTypeLookup));
                }
            }
        }

        return events;
    }

    private static async Task RemoveImmediateDispatchPositionAsync(EventStreamSlice toStore, Container container)
    {
        try
        {
            await container.Scripts.ExecuteStoredProcedureAsync<int>(
                StoredProcedures.RemoveImmediateDispatchPosition,
                new PartitionKey(toStore.StreamId),
                new dynamic[] { toStore.StreamId, toStore.Last().EventIndex });
        }
        catch (CosmosException e)
        {
            if (e.SubStatusCode != ConflictStatusCode)
            {
                throw;
            }
        }
    }
}