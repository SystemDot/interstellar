namespace Interstellar.Examples.Messages;

public class DestroyWotsit
{
    public Guid Id { get; }
    public Guid ThingId { get; }

    public DestroyWotsit(Guid id, Guid thingId)
    {
        Id = id;
        ThingId = thingId;
    }
}