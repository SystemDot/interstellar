﻿using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples;

using Interstellar.Examples.Cosmos;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterstellarExamples(this IServiceCollection services)
    {
        return services
            .AddSingleton<IEventDeliverer, MediatREventDeliverer>()
            .AddSingleton<UserService>()
            .AddSingleton<ExampleRunner>();
    }
}