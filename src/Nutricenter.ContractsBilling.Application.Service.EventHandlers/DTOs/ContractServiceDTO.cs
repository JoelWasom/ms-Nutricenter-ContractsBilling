using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.DTOs
{
    internal class ContractServiceDTO
    {
    }

    public sealed record ContractServiceCreateDTO(
        Guid ServiceTypeId,
        string ServiceId,
        string Description,
        decimal UnitPrice,
        int Quantity,
        int DurationDays
        );
}
