using System;
using Interstellar.Messaging;

namespace Interstellar.Domain.Bootstrapping
{
    public class DomainAggregateBootstrapping<TCommand, TAggregate>
        where TCommand : ICommand
        where TAggregate : AggregateRoot
    {
        public DomainAggregateBootstrapping<TCommand, TAggregate> ForId(Func<TCommand, string> idFactory)
        {
            throw new NotImplementedException();
        }
    }
}