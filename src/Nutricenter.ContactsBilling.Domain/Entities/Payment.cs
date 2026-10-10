using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Shared;
namespace Nutricenter.ContractsBilling.Domain.Entities
{
    public class Payment: Entity
    {
        public Payment() { }
        public Payment(Guid invoiceId,PaymentMethod metodoPago, AmountValue montoRecibido, AmountValue montoAplicado, string referencia)
        {
            
            Id = Guid.NewGuid();
            FechaPago = DateTime.Now;
            InvoiceId = invoiceId;
            MetodoPago = metodoPago;
            MontoRecibido = montoRecibido;
            MontoAplicado = montoAplicado;
            Referencia = referencia;
            Cambio.Value = (MontoRecibido?.Value ?? 0) - (MontoAplicado?.Value ?? 0);
        }

        //public Guid ServiceTypeId { get; set; }
        public Guid InvoiceId { get; set; }
        public DateTime FechaPago { get; set; }
        public PaymentMethod MetodoPago { get; set; }
        public AmountValue MontoRecibido { get; set; }
        public AmountValue MontoAplicado { get; set; }
        public AmountValue Cambio { get; set; }
        public string? Referencia { get; set; }
    }
}
