using System;

namespace Interstellar.Configuration
{
    public class DomainAggregateConfiguration<TAggregate> : DomainConfiguration
        where TAggregate : AggregateRoot
    {
        public DomainAggregateConfiguration(DomainConfiguration parent) : base(parent.Services)
        {
        }

        public DomainAggregateCommandConfiguration<TAggregate, TCommand> ReceiveCommand<TCommand>(
            Func<TCommand, string> streamIdFactory)
            where TCommand : class
        {
            AggregateLookupContext.Current.RegisterCommandForAggregateReception<TAggregate, TCommand>(streamIdFactory);
            return new DomainAggregateCommandConfiguration<TAggregate, TCommand>(this);
        }
    }
}