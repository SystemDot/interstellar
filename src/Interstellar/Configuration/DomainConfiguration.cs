using System;

namespace Interstellar.Configuration
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