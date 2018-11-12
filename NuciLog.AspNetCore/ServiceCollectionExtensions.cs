using System.IO;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Serilog;

namespace NuciLog.AspNetCore
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddNuciLogging(this IServiceCollection services)
        {
            IConfigurationBuilder configurationBuilder = new ConfigurationBuilder();
            IConfigurationRoot configurationRoot = configurationBuilder
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Logging.json")
                .Build();

            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configurationRoot).Enrich.FromLogContext().CreateLogger();
            services.AddSingleton(Log.Logger);

            return services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog(dispose: true));
        }
    }
}
