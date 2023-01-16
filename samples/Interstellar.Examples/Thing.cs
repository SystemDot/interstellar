using Interstellar.Configuration;
using Interstellar.Examples.Messages;

namespace Interstellar.Examples;

public class Thing : AggregateRoot
{
    public static void Configure(DomainConfiguration configuration)
    {
        configuration
            .ForAggregate<Thing>()
            .ReceiveCommand<CreateOrModifyThing>(command => command.Id.ToThingStreamId());
    }

    public Thing()
    {
        On<ThingCreated>().Become(Created);
        NotCreated();
    }

    private void NotCreated()
    {
        When<CreateOrModifyThing>().Then(command =>
            new ThingCreated(
                command.Id,
                command.Name,
                command.Description));
    }

    private void Created()
    {
        When<CreateOrModifyThing>().Then(command =>
            new ThingModified(
                command.Id,
                command.Name,
                command.Description));
    }
}