namespace Interstellar
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class EventStreamSlice : IEnumerable<EventPayload>
    {
        public string StreamId { get; }
        public long StartIndex { get; }
        public IEnumerable<EventPayload> Events { get; }

        public EventStreamSlice(string streamId) : this(streamId, -1)
        {
        }
        
        public EventStreamSlice(string streamId, long startIndex)
            : this(streamId, startIndex, new List<EventPayload>())
        {
        }

        private EventStreamSlice(string streamId, long startIndex, List<EventPayload> events)
        {
            StreamId = streamId;
            StartIndex = startIndex;
            Events = events;
        }

        public EventStreamSlice AddEvent(object toAdd)
        {
            List<EventPayload> events = Events.ToList();
            events.Add(new EventPayload(
                Guid.NewGuid(),
                StreamId, 
                (events.LastOrDefault()?.EventIndex ?? StartIndex) + 1,
                toAdd,
                DateTime.Now));

            return new EventStreamSlice(StreamId, StartIndex, events);
        }

        public IEnumerator<EventPayload> GetEnumerator() => Events.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}