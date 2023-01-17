using Interstellar;
using Interstellar.Examples;
using Interstellar.Examples.Messages;

public class ExampleRunner
{
    private readonly CosmosDatabaseProvider databaseProvider;
    private readonly DomainCommandDeliverer domainCommandDeliverer;

    public ExampleRunner(CosmosDatabaseProvider databaseProvider, DomainCommandDeliverer domainCommandDeliverer)
    {
        this.databaseProvider = databaseProvider;
        this.domainCommandDeliverer = domainCommandDeliverer;
    }

    public async Task RunAsync()
    {
        await databaseProvider.InitialiseAsync();

        var thingId = Guid.NewGuid();

        await domainCommandDeliverer.DeliverCommandAsync(new CreateOrModifyThing(
            thingId, 
            "CreatedThing", 
            "Created Thing"));

        await domainCommandDeliverer.DeliverCommandAsync(new CreateOrModifyThing(
            thingId,
            "ModifiedThing",
            "Modified Thing"));

        var wotsitId = Guid.NewGuid();

        await domainCommandDeliverer.DeliverCommandAsync(new StartWotsit(wotsitId, thingId));
        await domainCommandDeliverer.DeliverCommandAsync(new MakeWotsit(wotsitId, thingId, 50));
        await domainCommandDeliverer.DeliverCommandAsync(new DestroyWotsit(wotsitId, thingId));
    }
}