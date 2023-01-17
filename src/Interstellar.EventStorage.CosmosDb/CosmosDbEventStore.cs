namespace Interstellar.EventStorage.CosmosDb
{
    using Microsoft.Azure.Cosmos;
    
    public class CosmosDbEventStore : IEventStore
    {
        private const string PartitionKey = "/StreamId";
        private readonly IEventSourcingCosmosContainerProvider containerProvider;
        private readonly MessageNameTypeLookup messageNameTypeLookup;

        public CosmosDbEventStore(
            IEventSourcingCosmosContainerProvider containerProvider,
            MessageNameTypeLookup messageNameTypeLookup)
        {
            this.containerProvider = containerProvider;
            this.messageNameTypeLookup = messageNameTypeLookup;
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

            foreach (EventPayload eventPayload in toStore)
            {
                await container.CreateItemAsync(eventPayload.ToEventPayloadDataItem());
            }
        }
    }
}