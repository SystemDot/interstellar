using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.EventStorage.CosmosDb;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterstellarCosmosDbEventStorage<TEventSourcingCosmosContainerProvider>(
        this IServiceCollection services,
        CosmosDbEventStoreSettings settings)
        where TEventSourcingCosmosContainerProvider : class, IEventSourcingCosmosContainerProvider
    {
        services.AddSingleton(settings);
        services.AddSingleton<CosmosDatabaseInitialiser>();
        services.AddSingleton<IEventSourcingCosmosContainerProvider, TEventSourcingCosmosContainerProvider>();
        services.AddSingleton<IEventStore, CosmosDbEventStore>();
        return services;
    }
}