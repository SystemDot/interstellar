using Interstellar;
using Interstellar.Examples.Messages;

public class ExampleRunner
{
    private readonly DomainCommandDeliverer domainCommandDeliverer;

    public ExampleRunner(DomainCommandDeliverer domainCommandDeliverer)
    {
        this.domainCommandDeliverer = domainCommandDeliverer;
    }

    public async Task RunAsync()
    {
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