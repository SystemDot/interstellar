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

        public TAggregate CreateAggregate<TAggregate>() where TAggregate : AggregateRoot
        {
            return resolver.GetService<TAggregate>()!;
        }
    }
}