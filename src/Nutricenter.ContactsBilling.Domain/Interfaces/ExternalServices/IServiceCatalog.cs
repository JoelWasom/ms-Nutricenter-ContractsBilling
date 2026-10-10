using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices
{
    public interface IServiceCatalog
    {
        Task<ServiceDto?> GetServiceAsync(
        Guid serviceTypeId,
        string serviceId,
        CancellationToken cancellationToken = default);
    }

    public sealed class ServiceDto
    {
        public Guid ServiceTypeId { get; set; }
        public string ServiceId { get; init; } = string.Empty;

        public string Description { get; init; } = string.Empty;

        public decimal UnitPrice { get; init; }

        public int DurationDays { get; init; }

        public string Origin { get; init; } = string.Empty;
    }
}
