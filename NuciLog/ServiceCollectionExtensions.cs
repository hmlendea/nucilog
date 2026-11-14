using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NuciLog.Configuration;

namespace NuciLog
{
    /// <summary>
    /// Provides dependency injection registration helpers for the NuciLog services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Registers the NuciLog settings, binding them from the supplied configuration.
        /// </summary>
        /// <param name="services">The service collection to register the settings with.</param>
        /// <param name="configuration">The configuration that holds the settings section.</param>
        /// <returns>The same service collection, to allow fluent chaining.</returns>
        public static IServiceCollection AddNuciLoggerSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<NuciLoggerSettings>(
                configuration.GetSection(nameof(NuciLoggerSettings)));

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value);

            return services;
        }
    }
}
