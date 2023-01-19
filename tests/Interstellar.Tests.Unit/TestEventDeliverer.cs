namespace Interstellar.Tests.Unit;

public class TestEventDeliverer : IEventDeliverer
{
    private readonly DeliveredEventContext context;

    public TestEventDeliverer(DeliveredEventContext context)
    {
        this.context = context;
    }

    public Task DeliverAsync(EventPayload eventPayload)
    {
        context.AddEvent(eventPayload);
        return Task.CompletedTask;
    }
}