using AnagraficaAPI.Domain.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AnagraficaAPI.Configuration
{
    public static class MyConfigServiceCollectionExtensions
    {
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
            services.Configure<AppSettings>(config.GetSection(AppSettings.AppSettingsName));
            services.Configure<SecuritySettings>(config.GetSection(SecuritySettings.SecuritySettingsName));

            return services;
        }
    }
}
