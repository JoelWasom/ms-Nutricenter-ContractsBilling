using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("Invoices")]
    public class InvoiceStoreModel
    {
        [Key]
        public Guid InvoiceId { get; set; }

        public required Guid PacienteId { get; set; }
        public Guid ContractId { get; set; }

        [MaxLength(14)]
        public required string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateOnly FechaVencimiento {  get; set; }

        public required int Estado { get; set; } = 0;
        
        [MaxLength(512)]
        public string? Observaciones { get; set; }

        public List<InvoiceDetailStoreModel> Detalle { get; set; } = [];
        public List<PaymentStoredModel> Payments { get; set; } = [];
    }
}
