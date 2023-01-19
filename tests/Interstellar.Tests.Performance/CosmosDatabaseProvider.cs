using Microsoft.Azure.Cosmos;

namespace Interstellar.Tests.Performance;

public class CosmosDatabaseProvider
{
    private readonly CosmosDbSettings settings;
    private Database? database;

    public CosmosDatabaseProvider(CosmosDbSettings settings)
    {
        this.settings = settings;
    }

    public async Task InitialiseAsync()
    {
        CosmosClient client = CosmosConnector.ConnectToCosmos(settings.EndpointUri, settings.PrimaryKey);
        database = await client.CreateDatabaseIfNotExistsAsync(settings.Database);
    }

    public Database ProvideDatabase()
    {
        if (database == null)
        {
            throw new CosmosDatabaseProviderNotInitialisedException();
        }

        return database;
    }
}