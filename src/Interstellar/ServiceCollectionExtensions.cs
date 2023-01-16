using System;
using Interstellar.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterstellar(
            this IServiceCollection services,
            Action<DomainConfiguration> configureAction)
        {
            services.AddSingleton<IServiceProvider>(sp => sp);
            services.AddSingleton<AggregateFactory>();
            services.AddSingleton<EventStreamLoader>();
            services.AddSingleton<DomainCommandDeliverer>();

            var domainConfiguration = new DomainConfiguration(services);
            configureAction(domainConfiguration);

            return services;
        }
    }
}