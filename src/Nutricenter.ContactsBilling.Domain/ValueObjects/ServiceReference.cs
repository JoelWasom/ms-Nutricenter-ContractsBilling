using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Domain.ValueObjects
{
    public sealed partial class ServiceReference: ValueObject
    {
        public ServiceReference() { }
        public ServiceReference(string serviceId, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(serviceId)) { throw new DomainException(new Error("ServiceId", "ServiceId is required.", ErrorType.Validation)); }
            ServiceId = serviceId.Trim().ToUpperInvariant();
            Descripcion = descripcion.Trim();
        }
        public string ServiceId { get; set; }
        public string Descripcion { get; set; }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ServiceId;
            yield return Descripcion;
        }
    }
}
