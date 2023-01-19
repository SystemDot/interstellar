namespace Interstellar.EventStorage.CosmosDb;

public static class EventPayloadDataItemExtensions
{
    public static EventPayload ToEventPayload(
        this EventPayloadDataItem eventPayloadDataItem,
        MessageNameTypeLookup messageNameTypeLookup)
    {
        Type eventType = null!;
        var hasKnownBody = true;

        try
        {
            eventType = messageNameTypeLookup.Lookup(eventPayloadDataItem.EventTypeName);
        }
        catch (CannotLookupMessageTypeByNameException)
        {
            hasKnownBody = false;
        }
        
        var eventBody = hasKnownBody 
            ? eventPayloadDataItem.EventBody.FromJson(eventType)
            : eventPayloadDataItem.EventBody;

        return new EventPayload(
            Guid.Parse(eventPayloadDataItem.Id),
            eventPayloadDataItem.StreamId,
            eventPayloadDataItem.EventIndex,
            eventPayloadDataItem.Headers,
            eventBody,
            eventPayloadDataItem.CreatedOn);
    }
}