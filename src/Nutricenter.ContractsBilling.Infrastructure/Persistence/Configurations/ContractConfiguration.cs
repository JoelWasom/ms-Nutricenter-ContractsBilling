using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Configurations
{
    internal class ContractConfiguration: IEntityTypeConfiguration<Contract>,IEntityTypeConfiguration<ContractedService>
    {
        public void Configure(EntityTypeBuilder<Contract> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).HasColumnName("ContractId");

            entityBuilder.Property(x => x.PacienteId).IsRequired();
            entityBuilder.OwnsOne(x => x.NumeroContrato, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("NumeroContrato").IsRequired().HasMaxLength(14);
            });
            //entityBuilder.Property(x => x.NumeroContrato).IsRequired().HasMaxLength(14);
            entityBuilder.Property(x => x.FechaContrato).IsRequired();

            entityBuilder.OwnsOne(x => x.Periodo, owned =>
            {
                owned.Property(y => y.StartDate).HasColumnName("FechaInicio");
                owned.Property(y => y.EndDate).HasColumnName("FechaFin");
            });
            entityBuilder.Property(x => x.Estado).IsRequired();
            entityBuilder.Property(x => x.Observaciones).HasMaxLength(512);

            entityBuilder.HasMany(c => c.Services)
            .WithOne()
            .HasForeignKey(s => s.ContractId)
            .OnDelete(DeleteBehavior.Cascade);

            entityBuilder.Navigation(c => c.Services)
            .UsePropertyAccessMode(PropertyAccessMode.Field);


            //entityBuilder.entity<Contract>().HasMany(c => c.Services).WithOne().HasForeignKey(s => s.ContractId);

            //entityBuilder.Property(x => x.RowVersion).IsRowVersion().IsConcurrencyToken();

            entityBuilder.Ignore("_domainEvents");
            entityBuilder.Ignore(x => x.DomainEvents);
            entityBuilder.Ignore(x => x.MontoTotal);
        }
        public void Configure(EntityTypeBuilder<ContractedService> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            // El Id se genera en el constructor, no en la BD
            entityBuilder.Property(s => s.Id).ValueGeneratedNever();
            entityBuilder.Property(x => x.Id).HasColumnName("ContractServiceId").IsRequired();

            entityBuilder.Property(x => x.ContractId).IsRequired(); // 🔑 FK explícita
            entityBuilder.Property(x => x.ServiceTypeId).IsRequired();

            entityBuilder.OwnsOne(x => x.ServiceReferencia, owned =>
            {
                owned.Property(y => y.ServiceId).HasColumnName("ServiceId").IsRequired();
                owned.Property(y => y.Descripcion).HasColumnName("Descripcion").HasMaxLength(512);
            });

            entityBuilder.OwnsOne(x => x.Cantidad, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("Cantidad").IsRequired();
            });
            entityBuilder.OwnsOne(x => x.PrecioUnitario, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("PrecioUnitario").IsRequired();
                owned.Ignore(y => y.Currency);
            });
            entityBuilder.OwnsOne(x => x.DuracionDias, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("DuracionDias").IsRequired();
            });
        }
    }
}
