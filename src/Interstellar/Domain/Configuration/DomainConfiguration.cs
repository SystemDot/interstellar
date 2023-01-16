using System;

namespace Interstellar.Domain.Configuration
{
    public class DomainConfiguration
    {
        public DomainAggregateConfiguration<TAggregate> ForAggregate<TAggregate>()
            where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}