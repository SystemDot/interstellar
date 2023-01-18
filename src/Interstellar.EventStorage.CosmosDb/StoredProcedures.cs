namespace Interstellar.EventStorage.CosmosDb;

public static class StoredProcedures
{
    public const string EventStorage = "storeEventsSproc";
    public const string RemoveImmediateDispatchPosition = "removeImmediateDispatchPositionSproc";
}