using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Domain.Entities
{
    public  class ServiceType: AggregateRoot
    {
        public ServiceType() { }
        public ServiceType(string codigo, string nombre, bool isActive = true)
        {
            if (string.IsNullOrWhiteSpace(codigo))
            {
                throw new DomainException(new Error("NameIsRequired", "The item name is required", ErrorType.Validation));
            }
            Id = Guid.NewGuid();
            Codigo = codigo;
            Nombre = nombre;
            IsService = false;
            IsActive = isActive;
        }

        //public Guid ServiceTypeId { get; set; }
        public string Codigo { get; set; }
        public string Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? NombreMicroservicio { get; set; }
        public string? BaseUrl { get; set; }
        public string? Endpoint {  get; set; }
        public bool IsService { get; set; }
        public bool IsActive { get; set; } = true;
        public void Activate() => IsActive = true;
        public void Deactivate() => IsActive = false;
        public void Update(string name, string? description, string? microservice, string? url, string? endpoint, bool isService)
        {
            Nombre = name;
            Descripcion = description;
            NombreMicroservicio = microservice;
            BaseUrl = url;
            Endpoint = endpoint;
            IsService = isService;
        }
    }
}
