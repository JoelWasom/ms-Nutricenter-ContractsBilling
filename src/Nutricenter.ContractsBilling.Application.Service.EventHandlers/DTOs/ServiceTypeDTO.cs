using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.DTOs
{
    public class ServiceTypeDTO
    {
        public Guid ServiceTypeId { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public bool IsActive { get; set; }
    }
}
