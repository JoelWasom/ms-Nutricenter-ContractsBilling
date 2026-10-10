using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("Contracts")]
    public class ContractStoreModel
    {
        [Key]
        public Guid ContractId { get; set; }

        public required Guid PacienteId { get; set; }

        [MaxLength(14)]
        public required string NumeroContrato { get; set; }
        public DateTime FechaContrato { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin {  get; set; }

        public required int Estado { get; set; } = 0;
        
        [MaxLength(512)]
        public string? Observaciones { get; set; }

        public List<ContractedServiceStoredModel> Services { get; set; } = [];
    }
}
