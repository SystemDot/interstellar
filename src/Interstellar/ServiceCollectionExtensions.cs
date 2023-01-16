﻿using System;
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
            return services;
        }
    }
}