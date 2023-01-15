namespace Interstellar.Examples.Messages;

public class WotsitMade
{
    public Guid ThingId { get; }
    public Guid UserId { get; }
    public DateTime MadeOn { get; }
    public decimal Cost { get; }

    public WotsitMade(Guid thingId, Guid userId, DateTime madeOn, decimal cost)
    {
        ThingId = thingId;
        UserId = userId;
        MadeOn = madeOn;
        Cost = cost;
    }
}