using System;
using Interstellar.Messaging;

namespace Interstellar.Domain.Bootstrapping
{
    public class DomainCommandBootstrapping<TCommand> 
        where TCommand : ICommand
    {
        public DomainAggregateBootstrapping<TCommand, TAggregate> ToAggregate<TAggregate>()
            where TAggregate : AggregateRoot
        {
            throw new NotImplementedException();
        }
    }
}