using Microsoft.Extensions.DependencyInjection;
using Nutricenter.ContractsBilling.Domain;
using System.Reflection;

namespace Nutricenter.ContractsBilling.Application.Service
{
    public static class ApplicationServiceDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Registra la capa Domain
            services.AddDomain();

            // Configura MediatR para esta capa
            services.AddMediatR(config =>
            {
                config.RegisterServicesFromAssembly(typeof(ApplicationServiceDependencyInjection).Assembly);
            });

            return services;
        }
    }
}
