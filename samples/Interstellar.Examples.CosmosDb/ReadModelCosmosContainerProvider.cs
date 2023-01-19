using Microsoft.Azure.Cosmos;

namespace Interstellar.Examples.CosmosDb;

public class ReadModelCosmosContainerProvider
{
    private readonly CosmosContainerProvider thingList;
    private readonly CosmosContainerProvider wotsitActivity;

    public ReadModelCosmosContainerProvider(CosmosDatabaseProvider databaseProvider)
    {
        thingList = new CosmosContainerProvider(databaseProvider, "ThingList");
        wotsitActivity = new CosmosContainerProvider(databaseProvider, "WotsitActivity");
    }

    public Task<Container> ProvideThingListContainerAsync() =>
        thingList.ProvideContainerAsync("/Type");


    public Task<Container> ProvideWotsitActivityContainerAsync() =>
        wotsitActivity.ProvideContainerAsync("/ThingId");
}