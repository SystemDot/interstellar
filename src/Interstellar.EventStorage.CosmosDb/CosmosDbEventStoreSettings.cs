namespace Interstellar.EventStorage.CosmosDb;

public class CosmosDbEventStoreSettings
{
    public int WriteBatchSize { get; init; } = 300;
}