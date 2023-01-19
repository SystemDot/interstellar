namespace Interstellar.EventStorage.CosmosDb;

using Microsoft.Azure.Cosmos;

public class CosmosDbEventStore : IEventStore
{
    private static readonly int ConflictStatusCode = 409;

    private readonly IEventSourcingCosmosContainerProvider containerProvider;
    private readonly MessageNameTypeLookup messageNameTypeLookup;
    private readonly ImmediateEventDispatcher immediateEventDispatcher;
    private readonly CosmosDbEventStoreSettings settings;


    public CosmosDbEventStore(
        IEventSourcingCosmosContainerProvider containerProvider,
        MessageNameTypeLookup messageNameTypeLookup,
        ImmediateEventDispatcher immediateEventDispatcher,
        CosmosDbEventStoreSettings settings)
    {
        this.containerProvider = containerProvider;
        this.messageNameTypeLookup = messageNameTypeLookup;
        this.immediateEventDispatcher = immediateEventDispatcher;
        this.settings = settings;
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
        await immediateEventDispatcher.DispatchEventsAsync(toStore);
    }

    private async Task WriteEventsAsync(EventStreamSlice toStore, Container container)
    {
        try
        {
            var result = await container.Scripts.ExecuteStoredProcedureAsync<int>(
                StoredProcedures.EventStorage,
                new PartitionKey(toStore.StreamId),
                new dynamic[] { toStore.ToEventStreamSliceDataItem(settings.WriteBatchSize) });

            if (result.Resource < toStore.Events.Count())
            {
                await WriteEventsAsync(toStore.RemoveFirstNumberOfEvents(result.Resource), container);
            }
        }
        catch (CosmosException e)
        {
            if (IsOptimisticLockError(e))
            {
                throw new ExpectedEventIndexIncorrectException(toStore.StreamId, toStore.StartIndex);
            }
            throw;
        }
    }

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
                if (!Guid.TryParse(eventPayloadDataItem.Id, out _))
                {
                    continue;
                }
                
                events.Add(eventPayloadDataItem.ToEventPayload(messageNameTypeLookup));
            }
        }

        return events;
    }


    private Task<Container> GetContainerAsync() => 
        containerProvider.ProvideContainerAsync(PartitionKeys.StreamId);

    private static bool IsOptimisticLockError(CosmosException e) => 
        e.SubStatusCode == ConflictStatusCode && e.Message.Contains("Slice StartIndex:");
}