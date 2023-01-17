namespace Interstellar
{
    using System;

    public class EventPayload
    {
        public Guid Id { get; }
        public string StreamId { get; }
        public long EventIndex { get; }
        public object EventBody { get; }
        public DateTime CreatedOn { get; }

        public EventPayload(Guid id, string streamId, long eventIndex, object eventBody, DateTime createdOn)
        {
            Id = id;
            EventBody = eventBody;
            StreamId = streamId;
            EventIndex = eventIndex;
            CreatedOn = createdOn;
        }
    }
}