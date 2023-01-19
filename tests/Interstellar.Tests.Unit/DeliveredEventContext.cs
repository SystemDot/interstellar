namespace Interstellar.Tests.Unit;

public class DeliveredEventContext
{
    public List<EventPayload> Events { get; }

    public DeliveredEventContext()
    {
        Events = new List<EventPayload>();
    }

    public void AddEvent(EventPayload eventPayload)
    {
        Events.Add(eventPayload);
    }
}