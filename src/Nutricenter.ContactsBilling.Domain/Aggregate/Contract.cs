using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using System.Net.NetworkInformation;

namespace Nutricenter.ContractsBilling.Domain.Aggregate
{
    public class Contract : AggregateRoot
    {
        private readonly List<ContractedService> _services = new();
        private Contract() { }
        public Contract(Guid patientId, SequenceNumberGenerator contractNumber, ContractPeriod contractPeriod)
        {
            Id = Guid.NewGuid();
            PacienteId = patientId;
            NumeroContrato = contractNumber;
            FechaContrato = DateTime.Now;
            Periodo = contractPeriod;
            Estado = ContractStatus.Draft;
        }
        public Guid PacienteId { get; private set; }
        public SequenceNumberGenerator NumeroContrato { get; set; }
        public DateTime FechaContrato { get; set; }
        public ContractPeriod Periodo {  get; set; }
        public ContractStatus Estado { get; set; }
        public string? Observaciones {  get; set; }

        public IReadOnlyCollection<ContractedService> Services => _services.AsReadOnly();
        public AmountValue MontoTotal => new(_services.Sum(s => s.PrecioUnitario.Value));

        #region "Metodos para el Negocio"
        public void AddService(ContractedService service) 
        {
            if (Estado != ContractStatus.Draft)
                throw new DomainException(new Error("AddService","Only draft contracts can be modified.",ErrorType.Validation));
            if (_services.Any(x =>x.ServiceReferencia == service.ServiceReferencia))
                throw new DomainException(new Error("AddService", "The service already exists in the contract.", ErrorType.Validation));
            _services.Add(service);
        }
        public void RemoveService(Guid contractedServiceId)
        {
            if (Estado != ContractStatus.Draft)
                throw new DomainException(new Error("RemoveService", "Only draft contracts can be modified.", ErrorType.Validation));
            var service = _services.FirstOrDefault(x => x.Id == contractedServiceId) ?? throw new DomainException(new Error("RemoveService", "Service not found", ErrorType.Validation));
            _services.Remove(service);
        }
        public void Activate()
        {
            if (Estado != ContractStatus.Draft)
                throw new DomainException(new Error("ContractActive", "Only draft contracts can be activated.", ErrorType.Validation));
            if (_services.Count == 0)
                throw new DomainException(new Error("ContractActivate", "The contract must contrain at least one service", ErrorType.Validation));
            Estado = ContractStatus.Active;
        }
        public void Cancelled()
        {
            Estado = ContractStatus.Cancelled;
        }
        public void Terminated()
        {
            if (Estado != ContractStatus.Active)
                throw new DomainException(new Error("ContractTerminated", "Only active, current contracts may be terminated.", ErrorType.Validation));
            
            Estado = ContractStatus.Terminated;
        }
        public void Suspended()
        {
            if (Estado != ContractStatus.Active)
                throw new DomainException(new Error("ContractSuspended", "Only active, currently valid contracts may be suspended.", ErrorType.Validation));
            
            Estado = ContractStatus.Suspended;
        }

        //public byte[] RowVersion { get; private set; }
        #endregion
    }
}
