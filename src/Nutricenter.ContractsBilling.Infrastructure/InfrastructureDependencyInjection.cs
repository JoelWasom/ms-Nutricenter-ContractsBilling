using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Application.Service;
using Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.Interfaces.Services;
using Nutricenter.ContractsBilling.Infrastructure.ExternalServices.Catering;
using Nutricenter.ContractsBilling.Infrastructure.ExternalServices.Nutrition;
using Nutricenter.ContractsBilling.Infrastructure.Persistence;
using Nutricenter.ContractsBilling.Infrastructure.Persistence.Repositories;
using Nutricenter.ContractsBilling.Infrastructure.Services;
using Nutricenter.ContractsBilling.Infrastructure.Settings;
using System.Reflection;

namespace Nutricenter.ContractsBilling.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddApplication().AddPersistence(configuration);
            return services;
        }
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            var databaseSettings = configuration.GetSection(DatabaseSettings.SectionName)
                .Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database configuration not found.");

            services.AddDbContext<DomainDbContext>(options => options.UseSqlServer(databaseSettings.ConnectionString));

            services.AddDbContext<PersistenceDbContext>(options => options.UseSqlServer(databaseSettings.ConnectionString));

            // Repositorios y UoW → Scoped
            services.AddScoped<IDatabase, PersistenceDbContext>();

            services.AddScoped<IServiceTypeRepository, ServiceTypeRepository>();
            services.AddScoped<IContractRepository, ContractRepository>();
            services.AddScoped<IInvoicetRepository, InvoiceRepository>();
            services.AddScoped<IContractNumberGenerator, ContractNumberGenerator>();
            services.AddScoped<IInvoiceNumberGenerator, InvoiceNumberGenerator>();

            services.AddScoped<IServiceTypeQuery, ServiceTypeQuery>();
            //services.AddScoped<IServiceTypeQuery, ServiceTypeQuery>();
            //services.AddScoped<ITransactionRepository, TransactionRepository>();
            //services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddScoped<IServiceCatalog, JsonCateringProvider>();


            return services;
        }
        //public static IServiceCollection AddJsonCateringProvider(this IServiceCollection services, string dataPath)
        //{
        //    services.AddSingleton(new JsonCateringProvider(dataPath));
        //    return services;
        //}
        //public static IServiceCollection AddJsonNutritionProvider(this IServiceCollection services, string dataPath)
        //{
        //    services.AddSingleton(new JsonNutritionProvider(dataPath));
        //    return services;
        //}
    }
}
