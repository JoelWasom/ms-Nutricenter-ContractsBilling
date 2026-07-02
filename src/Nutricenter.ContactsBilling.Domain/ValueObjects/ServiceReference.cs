using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Domain.ValueObjects
{
    public sealed partial class ServiceReference: ValueObject
    {
        public ServiceReference() { }
        public ServiceReference(string serviceId, Guid serviceTypeId, string source)
        {
            if (string.IsNullOrWhiteSpace(serviceId)) { throw new DomainException(new Error("ServiceId", "ServiceId is required.", ErrorType.Validation)); }
        }

        public string ServiceId { get; set; }
        public Guid ServiceTypeId { get; set; }
        public string Source { get; set; }
        protected override IEnumerable<object?> GetEqualityComponents()
        {
            yield return ServiceId;
            yield return ServiceTypeId;
        }
    }
}
