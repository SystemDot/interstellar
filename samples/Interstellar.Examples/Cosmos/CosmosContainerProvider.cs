namespace Interstellar.Examples.Cosmos;

using Microsoft.Azure.Cosmos;

public class CosmosContainerProvider
{
    private readonly CosmosDatabaseProvider databaseProvider;
    private readonly string containerName;

    public CosmosContainerProvider(CosmosDatabaseProvider databaseProvider, string containerName)
    {
        this.databaseProvider = databaseProvider;
        this.containerName = containerName;
    }
    
    public async Task<Container> ProvideContainerAsync(string partitionKey)
    {
        return await databaseProvider
            .ProvideDatabase()
            .CreateContainerIfNotExistsAsync(containerName, partitionKey);
    }
}