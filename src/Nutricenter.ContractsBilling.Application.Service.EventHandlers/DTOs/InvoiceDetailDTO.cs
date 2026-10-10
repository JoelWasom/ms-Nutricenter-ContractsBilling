using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.DTOs
{
    internal class InvoiceDetailDTO
    {
    }

    public sealed record InvoiceDetailCreateDTO(
        string ServiceId,
        string Description,
        decimal UnitPrice,
        int Quantity
        );
}
