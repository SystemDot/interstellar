namespace Interstellar.EventStorage.InMemory
{
    using Microsoft.Extensions.DependencyInjection;

    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterstellarInMemoryEventStorage(
            this IServiceCollection services)
        {
            services.AddSingleton<IEventStore, InMemoryEventStore>();
            return services;
        }
    }
}