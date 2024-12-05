using FashionNest.Infrastructure.VnPay.DependencyInjection.Options.Config;
using FashionNest.Infrastructure.VnPay.Repository;
using FashionNest.Infrastructure.VnPay.Repository.Interface;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FashionNest.Infrastructure.VnPay.DependencyInjection.Extensions
{
    public static class ServiceCollectionExtension
    {
        public static IServiceCollection AddConfigInfrastructureVnPay(this IServiceCollection services)
        {
            services.AddTransient<IVnPayRepository, VnPayRepository>();
            return services;
        }

        public static OptionsBuilder<VnPayConfig> ConfigureVnPayOptions(this IServiceCollection services, IConfigurationSection section)
        {
            return services.AddOptions<VnPayConfig>()
                .Bind(section)
                .ValidateOnStart();
        }
    }
}
