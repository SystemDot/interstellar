using Microsoft.Extensions.DependencyInjection;
using System;

namespace Interstellar
{
    public class AggregateFactory
    {
        private readonly IServiceProvider resolver;

        public AggregateFactory(IServiceProvider resolver)
        {
            this.resolver = resolver;
        }

        public TAggregate CreateAggregate<TAggregate, TAggregateState>()
            where TAggregate : AggregateRoot<TAggregateState>
            where TAggregateState : AggregateState, new() =>
            resolver.GetService<TAggregate>()!;
    }
}