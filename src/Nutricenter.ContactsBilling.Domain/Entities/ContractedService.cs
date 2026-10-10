using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;

namespace Nutricenter.ContractsBilling.Domain.Entities
{
    public class ContractedService: Entity
    {
        private ContractedService() { }
        public ContractedService(Guid serviceTypeId, ServiceReference serviceReference, QuantityValue quantity, AmountValue unitPrice, QuantityValue durationDays) 
        {
            if (quantity <= 0) throw new DomainException(new Error("Quantity", "Quantity must be greater than zero.", ErrorType.Validation));
            if (durationDays <= 0) throw new DomainException(new Error("DurationDays", "Duration must be greater then zero.", ErrorType.Validation));

            Id = Guid.NewGuid();
            ServiceTypeId = serviceTypeId;
            ServiceReferencia = serviceReference;
            Cantidad = quantity;
            PrecioUnitario = unitPrice;
            DuracionDias = durationDays;
        }

        public Guid ContractId { get; set; }
        public Guid ServiceTypeId { get; set; }
        public ServiceReference ServiceReferencia { get; set; }
        public QuantityValue Cantidad { get; set; }
        public AmountValue PrecioUnitario { get; set; }
        public QuantityValue DuracionDias { get; set; }
        public AmountValue SubTotal => PrecioUnitario.Multiply(Cantidad);

        #region "Business Methods"
        public void ChangeQuantity(int quantity)
        {
            if (quantity <= 0)
                throw new DomainException(new Error("Quantity","Quantity must be greater than zero.",ErrorType.Validation));

            Cantidad = quantity;
        }

        public void ChangeUnitPrice(AmountValue price)
        {
            PrecioUnitario = price;
        }

        public void ChangeDuration(int durationDays)
        {
            if (durationDays <= 0)
                throw new DomainException(new Error("DuratioDays","Duration must be greater than zero.", ErrorType.Validation));

            DuracionDias = durationDays;
        }
        #endregion 
    }
}
