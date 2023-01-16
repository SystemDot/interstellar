using MediatR.Pipeline;
using Microsoft.Extensions.DependencyInjection;

namespace Interstellar.MediatR
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddInterstellarMediatR(this IServiceCollection services)
        {
            services.AddScoped(typeof(IRequestPreProcessor<>), typeof(GenericRequestPreProcessor<>));
            return services;
        }
    }
}