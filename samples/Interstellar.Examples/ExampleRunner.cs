using Interstellar.Examples.Messages;

namespace Interstellar.Examples;

public class ExampleRunner
{
    private readonly MessageBus domainCommandDeliverer;
    private readonly IEventStore eventStore;

    public ExampleRunner(MessageBus domainCommandDeliverer, IEventStore eventStore)
    {
        this.domainCommandDeliverer = domainCommandDeliverer;
        this.eventStore = eventStore;
    }

    public async Task RunAsync()
    {
        var thingId = Guid.NewGuid();

        await domainCommandDeliverer.SendCommandAsync(new CreateOrModifyThing(
            thingId, 
            "CreatedThing", 
            "Created Thing"));

        await domainCommandDeliverer.SendCommandAsync(new CreateOrModifyThing(
            thingId,
            "ModifiedThing",
            "Modified Thing"));

        var wotsitId = Guid.NewGuid();

        await domainCommandDeliverer.SendCommandAsync(new StartWotsit(wotsitId, thingId));
        await domainCommandDeliverer.SendCommandAsync(new MakeWotsit(wotsitId, thingId, 50));
        await domainCommandDeliverer.SendCommandAsync(new DestroyWotsit(wotsitId, thingId));
    }

    public async Task ReRunAsync()
    {
        await eventStore.RedeliverAllEventsAsync();
    }
}