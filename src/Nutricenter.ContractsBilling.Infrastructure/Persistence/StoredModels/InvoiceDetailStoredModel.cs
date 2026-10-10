using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("InvoiceDetails")]
    public class InvoiceDetailStoreModel
    {
        [Key]
        public Guid InvoiceDetailId { get; set; }

        public Guid InvoiceId { get; set; }
        public InvoiceStoreModel Invoice { get; set; }

        [MaxLength(128)]
        public required string ServiceId { get; set; }

        [MaxLength(512)]
        public string? Descripcion {  get; set; }

        public int Cantidad { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PrecioUnitario { get; set; }

    }
}
