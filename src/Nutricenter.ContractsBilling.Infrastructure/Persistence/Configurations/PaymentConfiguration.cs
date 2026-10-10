using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nutricenter.ContractsBilling.Domain.Entities;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Configurations
{
    public class PaymentConfiguration : IEntityTypeConfiguration<Payment>
    {
        
        public void Configure(EntityTypeBuilder<Payment> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).HasColumnName("PaymentId");

            entityBuilder.OwnsOne(x => x.MontoRecibido, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("MontoRecibido").IsRequired();
                owned.Ignore(y => y.Currency);
            });
            entityBuilder.OwnsOne(x => x.MontoAplicado, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("MontoAplicado").IsRequired();
                owned.Ignore(y => y.Currency);
            });
            entityBuilder.OwnsOne(x => x.Cambio, owned =>
            {
                owned.Property(y => y.Value).HasColumnName("Cambio").IsRequired();
                owned.Ignore(y => y.Currency);
            });
            entityBuilder.Property(x => x.Referencia).HasMaxLength(512);

            entityBuilder.Ignore("_domainEvents");
            entityBuilder.Ignore(x => x.DomainEvents);
        }
    }
}
