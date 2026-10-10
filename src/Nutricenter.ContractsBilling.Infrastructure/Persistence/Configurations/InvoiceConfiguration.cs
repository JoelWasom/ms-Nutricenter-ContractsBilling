using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Configurations
{
    internal class InvoiceConfiguration: IEntityTypeConfiguration<Invoice>, IEntityTypeConfiguration<InvoiceDetail>
    {
        public void Configure(EntityTypeBuilder<Invoice> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).HasColumnName("InvoiceId");

            entityBuilder.Property(x => x.PacienteId).IsRequired();
            entityBuilder.OwnsOne(x => x.NumeroFactura, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("NumeroFactura").IsRequired().HasMaxLength(14);
            });

            //entityBuilder.HasMany(c => c.Services)
            //.WithOne()
            //.HasForeignKey(s => s.InvoiceId)
            //.OnDelete(DeleteBehavior.Cascade);

            //entityBuilder.Navigation(c => c.Services)
            //.UsePropertyAccessMode(PropertyAccessMode.Field);

            entityBuilder.Ignore("_domainEvents");
            entityBuilder.Ignore(x => x.DomainEvents);
            entityBuilder.Ignore(x => x.MontoTotal);
        }
        public void Configure(EntityTypeBuilder<InvoiceDetail> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            // El Id se genera en el constructor, no en la BD
            entityBuilder.Property(s => s.Id).ValueGeneratedNever();
            entityBuilder.Property(x => x.Id).HasColumnName("InvoiceDetailId").IsRequired();

            entityBuilder.Property(x => x.InvoiceId).IsRequired(); // 🔑 FK explícita

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
        }
    }
}
