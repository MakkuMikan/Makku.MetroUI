using Makku.MetroUI.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Makku.MetroUI.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddMetroUI(this IServiceCollection services)
        {
            services.TryAddTransient(typeof(MetroHelper<>));

            return services;
        }
    }
}
