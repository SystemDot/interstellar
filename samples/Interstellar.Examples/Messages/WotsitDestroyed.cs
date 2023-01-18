namespace Interstellar.Examples.Messages;

using MediatR;

public class WotsitDestroyed : INotification
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