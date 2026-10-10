using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Nutricenter.ContractsBilling.Infrastructure.Persistence.StoredModels
{
    [Table("ServiceTypes")]
    public class ServiceTypeStoredModel
    {
        [Key]
        public Guid ServiceTypeId { get; set; }

        [MaxLength(4)]
        public required string Codigo { get; set; }

        [MaxLength(128)]
        public required string Nombre { get; set; }

        [MaxLength(256)]
        public string? Descripcion { get; set; }

        [MaxLength(256)]
        public string? NombreMicroservicio { get; set; }

        [MaxLength(256)]
        public string? BaseUrl { get; set; }

        [MaxLength(512)]
        public string? Endpoint { get; set; }
        public required bool IsService { get; set; }
        public required bool IsActive { get; set; } = true;
    }
}
