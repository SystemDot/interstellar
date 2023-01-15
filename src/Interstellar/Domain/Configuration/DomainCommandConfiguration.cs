using System;

namespace Interstellar.Domain.Configuration
{
    public class DomainCommandConfiguration<TCommand> : DomainConfiguration
    {
        public DomainAggregateConfiguration<TCommand, TAggregate> ToAggregate<TAggregate>(Func<TAggregate> aggregateCreator)
            where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}