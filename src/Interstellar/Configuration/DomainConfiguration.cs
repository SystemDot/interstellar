using System;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Configuration
{
    public class DomainConfiguration
    {
        public IServiceCollection Services { get; }

        public DomainConfiguration(IServiceCollection services)
        {
            Services = services;
        }

        public DomainAggregateConfiguration<TAggregate> ForAggregate<TAggregate>()
            where TAggregate : AggregateRoot
        {
            Services.AddTransient<TAggregate>();

            return new DomainAggregateConfiguration<TAggregate>(this);
        }
    }
}