namespace Interstellar.EventStorage.CosmosDb;

public class EventStreamSliceDataItem
{
    public string StreamId { get; init; } = null!;
    public long StartIndex { get; init; }
    public IEnumerable<EventPayloadDataItem> Events { get; init; } = null!;
}