namespace Interstellar.EventStorage.CosmosDb;

public static class EventPayloadDataItemExtensions
{
    public static EventPayload ToEventPayload(
        this EventPayloadDataItem eventPayloadDataItem,
        MessageNameTypeLookup messageNameTypeLookup)
    {
        return new EventPayload(
            Guid.Parse(eventPayloadDataItem.Id),
            eventPayloadDataItem.StreamId,
            eventPayloadDataItem.EventIndex,
            eventPayloadDataItem.EventBody.FromJson(messageNameTypeLookup.Lookup(eventPayloadDataItem.EventTypeName)),
            eventPayloadDataItem.CreatedOn);
    }
}