using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using NuciLog.Configuration;

namespace NuciLog
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNuciLoggerSettings(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            services.Configure<NuciLoggerSettings>(configuration.GetSection(nameof(NuciLoggerSettings)));

            services.AddSingleton(serviceProvider =>
                serviceProvider.GetRequiredService<IOptions<NuciLoggerSettings>>().Value);

            return services;
        }
    }
}