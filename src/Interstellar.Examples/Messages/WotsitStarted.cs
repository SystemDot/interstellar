namespace Interstellar.Examples.Messages;

public class WotsitStarted
{
    public Guid ThingId { get; }
    public Guid UserId { get; }
    public DateTime StartedOn { get; }

    public WotsitStarted(Guid thingId, Guid userId, DateTime startedOn)
    {
        ThingId = thingId;
        UserId = userId;
        StartedOn = startedOn;
    }
}