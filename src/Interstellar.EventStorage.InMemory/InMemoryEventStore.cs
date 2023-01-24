namespace Interstellar.EventStorage.InMemory
{
    using System.Collections.Concurrent;
    using System.Threading.Tasks;

    public class InMemoryEventStore : IEventStore
    {
        private readonly ConcurrentDictionary<string, EventStream> inner;
        private readonly IEventDeliverer eventDeliverer;
        private readonly object locker;

        public InMemoryEventStore(IEventDeliverer eventDeliverer)
        {
            this.eventDeliverer = eventDeliverer;
            inner = new ConcurrentDictionary<string, EventStream>();
            locker = new object();
        }

        public Task<EventStream> GetEventsAsync(string streamId)
        {
            lock (locker)
            {
                CreateStreamIfNonExistent(streamId);
                return Task.FromResult(inner[streamId]);
            }
        }

        public Task StoreEventsAsync(EventStreamSlice toStore)
        {
            lock (locker)
            {
                CreateStreamIfNonExistent(toStore.StreamId);
                var eventStream = inner[toStore.StreamId];
                CheckExpectedEventIndex(toStore, eventStream.CurrentEventIndex);
                inner[toStore.StreamId] = eventStream.Append(toStore);
            }

            return eventDeliverer.DeliverEventsAsync(toStore);
        }
        
        private void CheckExpectedEventIndex(EventStreamSlice toStore, long expectedIndex)
        {
            if (expectedIndex != toStore.StartIndex)
            {
                throw new ExpectedEventIndexIncorrectException(
                    toStore.StreamId,
                    expectedIndex,
                    toStore.StartIndex);
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
