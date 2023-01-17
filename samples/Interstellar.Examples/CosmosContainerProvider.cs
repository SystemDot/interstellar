using Microsoft.Azure.Cosmos;

namespace Interstellar.Examples;

public class CosmosContainerProvider
{
    private readonly CosmosDbSettings settings;
    private readonly CosmosDatabaseProvider databaseProvider;
    private readonly string containerName;

    public CosmosContainerProvider(CosmosDbSettings settings, CosmosDatabaseProvider databaseProvider, string containerName)
    {
        this.settings = settings;
        this.databaseProvider = databaseProvider;
        this.containerName = containerName;
    }
    
    public async Task<Container> ProvideContainerAsync()
    {
        return await databaseProvider
            .ProvideDatabase()
            .CreateContainerIfNotExistsAsync(containerName, settings.PartitionKey);
    }
}