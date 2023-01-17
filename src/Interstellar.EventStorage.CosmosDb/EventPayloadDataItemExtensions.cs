namespace Interstellar.EventStorage.CosmosDb;

public static class EventPayloadDataItemExtensions
{
    public static EventPayload ToEventPayload(this EventPayloadDataItem eventPayloadDataItem)
    {
        return new EventPayload(
            Guid.Parse(eventPayloadDataItem.Id),
            eventPayloadDataItem.StreamId,
            eventPayloadDataItem.EventIndex,
            eventPayloadDataItem.EventBody.FromJson(eventPayloadDataItem.EventTypeName),
            eventPayloadDataItem.CreatedOn);
    }
}