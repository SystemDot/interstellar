namespace Interstellar
{
    using System.Collections.Generic;
    using System.Linq;

    public class EventStream
    {
        public long CurrentEventIndex { get; }

        public IEnumerable<EventPayload> Events { get; }

        public EventStream() : this(new List<EventPayload>())
        {
        }

        public EventStream(IEnumerable<EventPayload> events)
        {
            Events = events;
            CurrentEventIndex = Events.LastOrDefault()?.EventIndex ?? -1;
        }

        public EventStream Append(EventStreamSlice toAppend)
        {

            List<EventPayload> events = Events.ToList();
            events.AddRange(toAppend.Events);
            return new EventStream(events);
        }

        public IEnumerable<EventPayload> SliceAt(long index)
        {
            return Events.Where(e => e.EventIndex > index);
        }
    }
}