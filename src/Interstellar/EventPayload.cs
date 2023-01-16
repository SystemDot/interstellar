namespace Interstellar
{
    using System;

    public class EventPayload
    {
        public string StreamId { get; }
        public long EventIndex { get; }
        public object Body { get; }
        public DateTime CreatedOn { get; }

        public EventPayload(string streamId, long eventIndex, object body, DateTime createdOn)
        {
            Body = body;
            StreamId = streamId;
            EventIndex = eventIndex;
            CreatedOn = createdOn;
        }
    }
}