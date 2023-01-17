using Microsoft.Azure.Cosmos;

namespace Interstellar.Examples;

public static class CosmosConnector
{
    public static CosmosClient ConnectToCosmos(string uri, string primaryKey)
    {
        return new CosmosClient(
            uri,
            primaryKey,
            new CosmosClientOptions
            {
                ApplicationName = "Interstellar.Examples"
            });
    }
}