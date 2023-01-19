namespace Interstellar.Examples;

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
        return domainCommandDeliverer.DeliverCommandAsync(command);
    }
}