using System;

namespace Interstellar.Domain.Configuration
{
    public class DomainAggregateConfiguration<TCommand, TAggregate> : DomainConfiguration
        where TAggregate : AggregateRoot
    {
        public DomainAggregateConfiguration<TCommand, TAggregate> ForId(Func<TCommand, string> idFactory)
        {
            throw new NotImplementedException();
        }
    }
}