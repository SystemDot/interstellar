using System;
using Interstellar.Configuration;

namespace Interstellar
{
    public class DomainAggregateCommandConfiguration<TAggregate, TCommand> : DomainAggregateConfiguration<TAggregate>
        where TAggregate : AggregateRoot
    {
        public DomainAggregateCommandConfiguration(DomainAggregateConfiguration<TAggregate> parent) : base(parent)
        {
        }

        public DomainAggregateCommandConfiguration<TAggregate, TCommand> JoinWithOtherStreams(params Func<TCommand, string>[] otherStreamIdFactories)
        {
            AggregateLookupContext.Current.SetAggregateToLookUpWithJoinedStreams(otherStreamIdFactories);
            return this;
        }
    }
}