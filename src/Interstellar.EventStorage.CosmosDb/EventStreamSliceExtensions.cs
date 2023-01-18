namespace Interstellar.EventStorage.CosmosDb;

public static class EventStreamSliceExtensions
{
    public static EventStreamSliceDataItem ToEventStreamSliceDataItem(this EventStreamSlice toStore)
    {
        return new EventStreamSliceDataItem
        {
            StreamId = toStore.StreamId,
            StartIndex = toStore.StartIndex,
            Events = toStore.Select(eventPayload => eventPayload.ToEventPayloadDataItem()),
        };
    }
}