namespace Interstellar.EventStorage.InMemory
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, EventStream> inner;
        private readonly IEventDeliverer eventDeliverer;

        public InMemoryEventStore(IEventDeliverer eventDeliverer)
        {
            this.eventDeliverer = eventDeliverer;
            inner = new ConcurrentDictionary<string, EventStream>();
        }

        public Task<EventStream> GetEventsAsync(string streamId)
        {
            lock (streamId)
            {
                CreateStreamIfNonExistent(streamId);
                return Task.FromResult(inner[streamId]);
            }
        }

        public Task StoreEventsAsync(EventStreamSlice toStore)
        {
            lock (toStore.StreamId)
            {
                CreateStreamIfNonExistent(toStore.StreamId);
                inner[toStore.StreamId] = inner[toStore.StreamId].Append(toStore);
            }

            return DeliverEventsAsync(toStore);
        }

        private async Task DeliverEventsAsync(EventStreamSlice toStore)
        {
            foreach (EventPayload? eventPayload in toStore)
            {
                await eventDeliverer.DeliverAsync(eventPayload);
            }
        }

        private void CreateStreamIfNonExistent(string streamId)
        {
            if (!inner.ContainsKey(streamId))
            {
                inner[streamId] = new EventStream();
            }
        }
    }
}
