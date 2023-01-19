namespace Interstellar.Tests.Unit;

using Microsoft.Extensions.DependencyInjection;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterstellarTests(
        this IServiceCollection services)
    {
        services.AddSingleton<DeliveredEventContext>();
        return services;
    }
}