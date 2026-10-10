using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("Payments")]
    public class PaymentStoredModel
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid InvoiceId { get; set; }
        public InvoiceStoreModel Invoice { get; set; }
        public DateTime FechaPago { get; set; }
        public int MetodoPago { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoRecibido { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal MontoAplicado { get; set; }
        [Column(TypeName = "decimal(18,2)")]
        public decimal Cambio { get; set; }

        [MaxLength(512)]
        public string? Referencia { get; set; }
    }
}
