using Microsoft.Extensions.DependencyInjection;

namespace Nutricenter.ContractsBilling.Domain
{
    public static class DomainDependencyInjection
    {
        public static IServiceCollection AddDomain(this IServiceCollection services)
        {
            // Servicios de dominio puros → Singleton

            //services.AddSingleton<IItemService, ItemService>();
            return services;
        }
    }
}
