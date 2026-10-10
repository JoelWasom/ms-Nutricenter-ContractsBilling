using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;

namespace Nutricenter.ContractsBilling.Domain.Entities
{
    public class InvoiceDetail: Entity
    {
        private InvoiceDetail() { }
        public InvoiceDetail(ServiceReference serviceReference, QuantityValue quantity, AmountValue unitPrice)
        {
            if (quantity <= 0) throw new DomainException(new Error("Quantity", "Quantity must be greater than zero.", ErrorType.Validation));

            Id = Guid.NewGuid();
            ServiceReferencia = serviceReference;
            Cantidad = quantity;
            PrecioUnitario = unitPrice;
        }

        public Guid InvoiceId { get; set; }
        public ServiceReference ServiceReferencia { get; set; }
        public QuantityValue Cantidad { get; set; }
        public AmountValue PrecioUnitario { get; set; }
        public AmountValue SubTotal => PrecioUnitario.Multiply(Cantidad);

        #region "Business Methods"
        public void ChangeQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException(new Error("Quantity", "Quantity must be greater than zero.", ErrorType.Validation));
            Cantidad = quantity;
        }
        public void ChangeUnitPrice(AmountValue price)
        {
            PrecioUnitario = price;
        }
        #endregion 
    }
}
