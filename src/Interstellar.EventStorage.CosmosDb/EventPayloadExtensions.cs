namespace Interstellar.EventStorage.CosmosDb;

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