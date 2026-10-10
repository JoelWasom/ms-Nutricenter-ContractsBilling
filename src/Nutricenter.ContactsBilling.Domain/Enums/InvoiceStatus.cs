using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Domain.Enums
{
    public enum InvoiceStatus
    {
        Pending = 1,
        Paid = 2,
        Partial = 3,
        Cancelled = 4,
        Expired = 5,
    }
}
