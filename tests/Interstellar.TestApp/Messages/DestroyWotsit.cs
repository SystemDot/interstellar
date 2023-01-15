namespace Interstellar.TestApp.Messages;

public class DestroyWotsit
{
    public string Id { get; }
    public Guid ThingId { get; }

    public DestroyWotsit(string id, Guid thingId)
    {
        Id = id;
        ThingId = thingId;
    }
}