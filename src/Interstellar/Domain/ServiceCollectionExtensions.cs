using System;
using Interstellar.Domain.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Domain
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterstellar(
            this IServiceCollection services,
            Action<DomainConfiguration> configureAction)
        {
            return services;
        }
    }
}