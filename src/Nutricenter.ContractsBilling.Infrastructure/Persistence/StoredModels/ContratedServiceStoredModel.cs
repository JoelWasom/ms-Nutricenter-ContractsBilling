using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("ContractedServices")]
    public class ContractedServiceStoredModel
    {
        [Key]
        public Guid ContractServiceId { get; set; }

        public Guid ContractId { get; set; }
        public ContractStoreModel Contract { get; set; }

        public Guid ServiceTypeId { get; set; }

        [MaxLength(256)]
        public required string ServiceId { get; set; }

        [MaxLength(512)]
        public string? Descripcion {  get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }
        public int DuracionDias { get; set; }

    }
}
