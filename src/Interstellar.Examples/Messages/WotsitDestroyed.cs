namespace Interstellar.Examples.Messages;

public class WotsitDestroyed
{
    public Guid ThingId { get; }
    public Guid UserId { get; }
    public DateTime WithdrawnOn { get; }

    public WotsitDestroyed(Guid thingId, Guid userId, DateTime withdrawnOn)
    {
        ThingId = thingId;
        UserId = userId;
        WithdrawnOn = withdrawnOn;
    }
}