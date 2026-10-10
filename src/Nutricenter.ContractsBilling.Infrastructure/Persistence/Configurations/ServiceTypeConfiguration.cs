using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Nutricenter.ContractsBilling.Domain.Entities;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.Configurations
{
    public class ServiceTypeConfiguration : IEntityTypeConfiguration<ServiceType>
    {
        public void Configure(EntityTypeBuilder<ServiceType> entityBuilder)
        {
            entityBuilder.HasKey(x => x.Id);
            entityBuilder.Property(x => x.Id).HasColumnName("ServiceTypeId");

            entityBuilder.Property(x => x.Codigo).IsRequired().HasMaxLength(4);
            entityBuilder.Property(x => x.Nombre).IsRequired().HasMaxLength(128);
            entityBuilder.Property(x => x.Descripcion).HasMaxLength(256);
            entityBuilder.Property(x => x.NombreMicroservicio).HasMaxLength(256);
            entityBuilder.Property(x => x.BaseUrl).HasMaxLength(256);
            entityBuilder.Property(x => x.Endpoint).HasMaxLength(512);

            entityBuilder.Property(x => x.IsService).IsRequired();
            entityBuilder.Property(x => x.IsActive).HasDefaultValue(true);

            entityBuilder.Ignore("_domainEvents");
            entityBuilder.Ignore(x => x.DomainEvents);
        }
    }
}
