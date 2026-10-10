using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using Nutricenter.ContractsBilling.Infrastructure.Settings;
namespace Nutricenter.ContractsBilling.Infrastructure.Persistence
{
    public class PersistenceDbContextFactory : IDesignTimeDbContextFactory<PersistenceDbContext>
    {
        public PersistenceDbContext CreateDbContext(string[] args)
        {
            // Construir IConfiguration manualmente en tiempo de diseño
            var config = new ConfigurationBuilder()
                .SetBasePath(Path.Combine(Directory.GetCurrentDirectory(), "../Nutricenter.ContractsBilling.WebApi")) // apunta al proyecto de inicio (WebApi)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

            var databaseSettings = config.GetSection(DatabaseSettings.SectionName)
                .Get<DatabaseSettings>() ?? throw new InvalidOperationException("Database configuration not found.");

            var optionsBuilder = new DbContextOptionsBuilder<PersistenceDbContext>();
            optionsBuilder.UseSqlServer(databaseSettings.ConnectionString);


            optionsBuilder.UseSqlServer(databaseSettings.ConnectionString);

            return new PersistenceDbContext(optionsBuilder.Options);
        }
    }
}
