using Microsoft.EntityFrameworkCore;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;
using System.Reflection;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence
{
    internal class DomainDbContext(DbContextOptions<DomainDbContext> options) : DbContext(options)
    {
        public DbSet<ServiceType> ServiceTypes { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<ContractedService> ContractedServices { get; set; }
        public DbSet<Payment> Payments { get; set; }
        public DbSet<Invoice> Invoices { get; set; }
        public DbSet<InvoiceDetail> InvoiceDetails { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            base.OnModelCreating(modelBuilder);

            modelBuilder.Ignore<DomainEvent>();
        }
    }
}
