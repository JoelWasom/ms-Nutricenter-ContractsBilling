using Nutricenter.ContractsBilling.Domain.Shared;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Domain.Aggregates.Contract
{
    public class ContractedService
    {
        private ContractedService() { }
        public ContractedService(ServiceReference) 
        {
            
        }
        public Guid ContractServiceId { get; set; }
        public string Descripcion { get; set; }
        public QuantityValue Cantidad { get; set; }
        public AmountValue PrecioUnitario { get; set; }
        public QuantityValue DuracionDias { get; set; }
    }
}
