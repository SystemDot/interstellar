using System;

namespace Interstellar.Configuration
{
    public class DomainAggregateConfiguration<TAggregate, TAggregateState> : DomainConfiguration
        where TAggregate : AggregateRoot<TAggregateState>
        where TAggregateState : AggregateState, new()
    {
        public DomainAggregateConfiguration(DomainConfiguration parent) : base(parent.Services, parent.AggregateLookup)
        {
        }

        public DomainAggregateCommandConfiguration<TAggregate, TAggregateState, TCommand> ReceiveCommand<TCommand>(
            Func<TCommand, string> streamIdFactory)
            where TCommand : class
        {
            AggregateLookup.RegisterCommandForAggregateReception<TAggregate, TAggregateState, TCommand>(streamIdFactory);
            return new DomainAggregateCommandConfiguration<TAggregate, TAggregateState, TCommand>(this);
        }
    }
}