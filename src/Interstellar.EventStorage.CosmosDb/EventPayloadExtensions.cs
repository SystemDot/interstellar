namespace Interstellar.EventStorage.CosmosDb;

public static class EventPayloadExtensions
{
    public static EventPayloadDataItem ToEventPayloadDataItem(this EventPayload eventPayload) =>
        new()
        {
            Id = eventPayload.Id.ToString(),
            StreamId = eventPayload.StreamId,
            EventIndex = eventPayload.EventIndex,
            EventTypeName = eventPayload.EventBody.GetType().Name!,
            EventBody = eventPayload.EventBody.ToJson(),
            CreatedOn = eventPayload.CreatedOn
        };
}