using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.Examples;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInterstellarExamples(this IServiceCollection services)
    {
        return services
            .AddSingleton<MessageBus>()
            .AddSingleton<UserService>()
            .AddSingleton<ExampleRunner>();
    }
}