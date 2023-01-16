namespace Interstellar.MediatR;

using global::MediatR;

public class MediatREventDeliverer : IEventDeliverer
{
    private readonly IMediator mediator;

    public MediatREventDeliverer(IMediator mediator)
    {
        this.mediator = mediator;
    }

    public Task DeliverAsync(EventPayload eventPayload) => mediator.Publish(eventPayload.Body);
}