using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Interstellar.EventStorage.CosmosDb
{
    using Microsoft.Azure.Cosmos;


    public class EventPayloadDataItem
    {
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        public string StreamId { get; set; }
        
        public long EventIndex { get; set; }
        
        public string EventTypeName { get; set; }
        
        public JObject EventBody { get; set; }

        public DateTime CreatedOn { set; get; }
    }

    public static class EventPayloadDataItemExtensions
    {
        public static EventPayload ToEventPayload(this EventPayloadDataItem eventPayloadDataItem)
        {
            return new EventPayload(
                Guid.Parse(eventPayloadDataItem.Id),
                eventPayloadDataItem.StreamId,
                eventPayloadDataItem.EventIndex,
                eventPayloadDataItem.EventBody.FromJson(eventPayloadDataItem.EventTypeName),
                eventPayloadDataItem.CreatedOn);
        }
    }

    public static class EventPayloadExtensions
    {
        public static EventPayloadDataItem ToEventPayloadDataItem(this EventPayload eventPayload)
        {
            return new EventPayloadDataItem
            {
                Id = eventPayload.Id.ToString(),
                StreamId = eventPayload.StreamId,
                EventIndex = eventPayload.EventIndex,
                EventTypeName = eventPayload.EventBody.GetType().AssemblyQualifiedName!,
                EventBody = eventPayload.EventBody.ToJson(),
                CreatedOn = eventPayload.CreatedOn
            };
        }
    }

    public static class StringExtensions
    {
        public static object FromJson(this JObject from, string eventTypeName)
        {
            return from.ToObject(Type.GetType(eventTypeName));
        }
    }

    public static class ObjectExtensions
    {
        public static JObject ToJson(this object from)
        {
            return JObject.FromObject(from);
        }
    }

    public class CosmosDbEventStore : IEventStore
    {
        private readonly IEventSourcingCosmosContainerProvider containerProvider;

        public CosmosDbEventStore(IEventSourcingCosmosContainerProvider containerProvider)
        {
            this.containerProvider = containerProvider;
        }

        public async Task<EventStream> GetEventsAsync(string streamId)
        {
            var container = await containerProvider.ProvideContainerAsync();
            
            var queryDefinition = new QueryDefinition($"SELECT * FROM c WHERE c.StreamId = '{streamId}'");
            var queryResultSetIterator = container.GetItemQueryIterator<EventPayloadDataItem>(queryDefinition);

            var events = new List<EventPayload>();

            while (queryResultSetIterator.HasMoreResults)
            {
                var currentResultSet = await queryResultSetIterator.ReadNextAsync();
                foreach (EventPayloadDataItem? eventPayloadDataItem in currentResultSet)
                {
                    events.Add(eventPayloadDataItem.ToEventPayload());
                }
            }

            return new EventStream(events);
        }

        public async Task StoreEventsAsync(EventStreamSlice toStore)
        {
            var container = await containerProvider.ProvideContainerAsync();

            foreach (EventPayload eventPayload in toStore)
            {
                await container.CreateItemAsync(eventPayload.ToEventPayloadDataItem());
            }

        }
    }
}