namespace Interstellar
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EventStreamLoader
    {
        private readonly IEventStore store;

        public EventStreamLoader(IEventStore store)
        {
            this.store = store;
        }

        public async Task<IEnumerable<EventPayload>> LoadEventsAsync(IEnumerable<string> streamIds)
        {
            var result = new List<EventPayload>();
            var streamCount = 0;

            foreach (string streamId in streamIds)
            {
                streamCount++;
                EventStream eventStream = await store.GetEventsAsync(streamId);
                result.AddRange(eventStream.Events);
            }

            if (streamCount > 1)
            {
                return result
                    .OrderBy(e => e.CreatedOn)
                    .ThenBy(e => e.StreamId)
                    .ThenBy(e => e.EventIndex);
            }

            return result;
        }
    }
}