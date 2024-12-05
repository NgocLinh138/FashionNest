using FashionNest.Application.Mapper;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace FashionNest.Application.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());
            });

            services.AddAutoMapper(typeof(ServiceProfile).Assembly);

            return services;
        }
    }
}
