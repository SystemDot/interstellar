using System;
using Interstellar.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar
{
    public static class ServiceCollectionExtensions
    {
        
        public static IServiceCollection AddInterstellar<TEventDeliverer>(
            this IServiceCollection services,
            Action<DomainConfiguration> configureAction,
            MessageNameTypeLookup lookup)
            where TEventDeliverer : class, IEventDeliverer
        {
            services
                .AddSingleton<IEventDeliverer, TEventDeliverer>()
                .AddSingleton<IServiceProvider>(sp => sp)
                .AddSingleton(lookup)
                .AddSingleton<AggregateFactory>()
                .AddSingleton<EventStreamLoader>()
                .AddSingleton<DomainCommandDeliverer>();

            var domainConfiguration = new DomainConfiguration(services);
            configureAction(domainConfiguration);

            return services;
        }
    }
}