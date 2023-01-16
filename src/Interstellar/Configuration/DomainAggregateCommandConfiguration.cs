﻿using System;

namespace Interstellar.Configuration
{
    public class DomainAggregateCommandConfiguration<TAggregate, TCommand> : DomainAggregateConfiguration<TAggregate>
        where TAggregate : AggregateRoot
    {

        public DomainAggregateCommandConfiguration<TAggregate, TCommand> JoinWithOtherStreams(params Func<TCommand, string>[] otherStreamIdFactories)
        {
            throw new NotImplementedException();
        }
    }
}