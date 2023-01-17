namespace Interstellar.EventStorage.CosmosDb;

public static class EventStreamSliceExtensions
{
    public static ImmediateDispatchPosition ToImmediateDispatchPosition(this EventStreamSlice toStore)
    {
        return new ImmediateDispatchPosition
        {
            Id = toStore.StreamId,
            StreamId = toStore.StreamId,
            LastIndexDispatched = toStore.StartIndex,
        };
    }
}