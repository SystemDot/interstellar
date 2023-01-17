using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterstellarExamples(
        this IServiceCollection services,
        CosmosDbSettings cosmosDbSettings)
    {
        return services.AddSingleton(cosmosDbSettings!)
            .AddSingleton<CosmosDatabaseProvider>()
            .AddSingleton<IEventDeliverer, MediatREventDeliverer>()
            .AddSingleton<UserService>()
            .AddSingleton<ExampleRunner>();
    }
}