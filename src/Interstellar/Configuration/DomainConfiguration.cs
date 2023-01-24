using System;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Configuration
{
    public class DomainConfiguration
    {
        internal AggregateLookup AggregateLookup { get; }

        internal IServiceCollection Services { get; }

        public DomainConfiguration(IServiceCollection services, AggregateLookup aggregateLookup)
        {
            AggregateLookup = aggregateLookup;
            Services = services;
        }

        public DomainAggregateConfiguration<TAggregate, NullState> ForAggregate<TAggregate>()
            where TAggregate : AggregateRoot
        {
            Services.AddTransient<TAggregate>();

            return new DomainAggregateConfiguration<TAggregate, NullState>(this);
        }

        public DomainAggregateConfiguration<TAggregate, TAggregateState> ForAggregate<TAggregate, TAggregateState>()
            where TAggregate : AggregateRoot<TAggregateState>
            where TAggregateState : AggregateState, new()
        {
            Services.AddTransient<TAggregate>();

            return new DomainAggregateConfiguration<TAggregate, TAggregateState>(this);
        }
    }
}