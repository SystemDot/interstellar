namespace Interstellar
{
    using System.Collections.Generic;
    using System.Linq;

    public class EventStream
    {
        private readonly long currentEventIndex;
        public IEnumerable<EventPayload> Events { get; }

        public EventStream() : this(new List<EventPayload>())
        {
        }

        public EventStream(IEnumerable<EventPayload> events)
        {
            Events = events;
            currentEventIndex = Events.LastOrDefault()?.EventIndex ?? -1;
        }

        public EventStream Append(EventStreamSlice toAppend)
        {
            if (currentEventIndex != toAppend.StartIndex)
            {
                throw new ExpectedEventIndexIncorrectException(toAppend.StreamId, currentEventIndex, toAppend.StartIndex);
            }

            List<EventPayload> events = Events.ToList();
            events.AddRange(toAppend.Events);
            return new EventStream(events);
        }
    }
}