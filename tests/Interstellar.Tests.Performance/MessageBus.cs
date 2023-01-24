namespace Interstellar.Tests.Performance;

public class MessageBus
{
    private readonly DomainCommandDeliverer domainCommandDeliverer;
    private readonly AggregateLookup aggregateLookup;

    public MessageBus(DomainCommandDeliverer domainCommandDeliverer, AggregateLookup aggregateLookup)
    {
        this.domainCommandDeliverer = domainCommandDeliverer;
        this.aggregateLookup = aggregateLookup;
    }

    public Task SendCommandAsync<TCommand>(TCommand command)
    {
        if(!aggregateLookup.CommandIsReceivedByAnAggregate<TCommand>())
        {
            // Go to some other endpoint to send command
        }

        var headers = new Dictionary<string, object> { { "correlationId", Guid.NewGuid() } };
        return domainCommandDeliverer.DeliverCommandAsync(command, headers);
    }
}