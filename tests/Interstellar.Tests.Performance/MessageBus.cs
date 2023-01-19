namespace Interstellar.Tests.Performance;

public class MessageBus
{
    private readonly DomainCommandDeliverer domainCommandDeliverer;
    
    public MessageBus(DomainCommandDeliverer domainCommandDeliverer)
    {
        this.domainCommandDeliverer = domainCommandDeliverer;
    }

    public Task SendCommandAsync<TCommand>(TCommand command)
    {
        if(!AggregateLookupContext.Current.CommandIsReceivedByAnAggregate<TCommand>())
        {
            // Go to some other endpoint to send command
        }

        var headers = new Dictionary<string, object> { { "correlationId", Guid.NewGuid() } };
        return domainCommandDeliverer.DeliverCommandAsync(command, headers);
    }
}