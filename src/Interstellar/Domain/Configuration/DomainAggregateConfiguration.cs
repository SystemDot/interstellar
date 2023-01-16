using System;
using System.Collections.Generic;

namespace Interstellar.Domain.Configuration
{
    public class DomainAggregateConfiguration<TAggregate> : DomainConfiguration
        where TAggregate : AggregateRoot
    {
        
        public DomainAggregateCommandConfiguration<TAggregate, TCommand> ReceiveCommand<TCommand>(Func<TCommand, string> streamIdFactory)
        {
            throw new NotImplementedException();
        }
    }
}