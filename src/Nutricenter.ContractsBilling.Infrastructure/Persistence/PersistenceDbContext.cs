using Microsoft.EntityFrameworkCore;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence
{
    public class PersistenceDbContext: DbContext, IDatabase
    {
        public PersistenceDbContext(DbContextOptions<PersistenceDbContext> options):base(options) 
        {
        }
        public DbSet<ServiceTypeStoredModel> ServiceTypes { get; set; }
        public DbSet<ContractStoreModel> Contracts { get; set; }
        public DbSet<ContractedServiceStoredModel> ContractedServices { get; set; }
        public DbSet<PaymentStoredModel> Payments { get; set; }
        public DbSet<InvoiceStoreModel> Invoices { get; set; }
        public DbSet<InvoiceDetailStoreModel> InvoiceDetails { get; set; }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public void Migrate()
        {
            Database.Migrate();
        }
    }
}
