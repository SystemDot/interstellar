using System;
using Interstellar.Configuration;

namespace Interstellar
{
    public class DomainAggregateCommandConfiguration<TAggregate, TAggregateState, TCommand> : DomainAggregateConfiguration<TAggregate, TAggregateState>
        where TAggregate : AggregateRoot<TAggregateState>
        where TAggregateState : AggregateState, new()
    {
        public DomainAggregateCommandConfiguration(DomainAggregateConfiguration<TAggregate, TAggregateState> parent) : base(parent)
        {
        }

        public DomainAggregateCommandConfiguration<TAggregate, TAggregateState, TCommand> JoinWithOtherStreams(params Func<TCommand, string>[] otherStreamIdFactories)
        {
            AggregateLookupContext.Current.SetAggregateToLookUpWithJoinedStreams(otherStreamIdFactories);
            return this;
        }
    }
}