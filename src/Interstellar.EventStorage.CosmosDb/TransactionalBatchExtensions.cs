using Microsoft.Azure.Cosmos;

namespace Interstellar.EventStorage.CosmosDb;

public static class TransactionalBatchExtensions
{
    public static void WriteEventsToStream(this TransactionalBatch batch, EventStreamSlice toStore)
    {
        foreach (EventPayload eventPayload in toStore)
        {
            batch.CreateItem(eventPayload.ToEventPayloadDataItem());
        }
    }

    public static void WriteImmediateDispatchPosition(this TransactionalBatch batch, EventStreamSlice toStore)
    {
        batch.CreateItem(toStore.ToImmediateDispatchPosition());
    }
}