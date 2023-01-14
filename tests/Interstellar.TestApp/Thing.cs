using Interstellar.Domain;
using Interstellar.Domain.Bootstrapping;

public class Thing : AggregateRoot
{
    public static void Bootstrap()
    {
        DomainBootstrapping
            .Route<CreateOrModifyThing>()
            .ToAggregate<Thing>()
            .ForId(command => $"{nameof(Thing)}-{command.Id}");
    }

    public Thing()
    {
        On<ThingCreated>().Become(Created);
        NotCreated();
    }

    public void NotCreated()
    {
        When<CreateOrModifyThing>().Then(command =>
            new ThingCreated(
                command.Id,
                command.Name,
                command.Description));
    }

    public void Created()
    {
        When<CreateOrModifyThing>().Then(command =>
            new ThingModified(
                command.Id,
                command.Name,
                command.Description));
    }
}