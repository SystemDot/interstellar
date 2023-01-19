using System.Collections.Generic;

namespace Interstellar
{
    using System;

    public class EventPayload
    {
        public Guid Id { get; }
        public string StreamId { get; }
        public long EventIndex { get; }
        public IDictionary<string, object> Headers { get; }
        public object EventBody { get; }
        public DateTime CreatedOn { get; }

        public EventPayload(
            Guid id,
            string streamId,
            long eventIndex,
            IDictionary<string, object> headers,
            object eventBody,
            DateTime createdOn)
        {
            Id = id;
            EventBody = eventBody;
            StreamId = streamId;
            EventIndex = eventIndex;
            Headers = headers;
            CreatedOn = createdOn;
        }
    }
}