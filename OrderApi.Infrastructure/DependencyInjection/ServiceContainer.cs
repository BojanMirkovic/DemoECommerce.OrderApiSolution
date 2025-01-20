

using eCommerce.SharedLibrary.DependencyInjection;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderApi.Aplication.Interfaces;
using OrderApi.Infrastructure.Database;
using OrderApi.Infrastructure.Repositories;

namespace OrderApi.Infrastructure.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddInfrastructureService(this IServiceCollection service, IConfiguration config)
        {
            //Add Database Connectivity
            //Add Authentication scheme
            SharedServiceContainer.AddSharedServices<OrderDbContext>(service, config, config["MySerilog:FileName"]!);

            //Create DI
            service.AddScoped<IOrder, OrderRepository>();
            return service;
        }

        public static IApplicationBuilder UserInfrastracturePolicy(this IApplicationBuilder app)
        {
            //Register middleware such as:
            //Global Exception Only -> handle external errors
            //ListenToApiGateway Only -> block all outside calls
            SharedServiceContainer.UseSharedPolicies(app);
            return app;

        }
    }
}
