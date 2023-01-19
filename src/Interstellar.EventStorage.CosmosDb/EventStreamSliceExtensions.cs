namespace Interstellar.EventStorage.CosmosDb;

public static class EventStreamSliceExtensions
{
    public static EventStreamSliceDataItem ToEventStreamSliceDataItem(this EventStreamSlice toStore, int batchSize)
    {
        return new EventStreamSliceDataItem
        {
            StreamId = toStore.StreamId,
            StartIndex = toStore.StartIndex,
            Events = toStore
                .Take(batchSize)
                .Select(eventPayload => eventPayload.ToEventPayloadDataItem()),
        };
    }

    public static string ToImmediateUpdateEventIndexId(this EventStreamSlice toStore) =>
        $"immediate-update-event-index-{toStore.StreamId}-{toStore.StartIndex}";
}