using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace Nutricenter.ContractsBilling.Domain.Aggregates.Contract
{
    public class Contract : AggregateRoot
    {
        private readonly List<ContractedService> _services = new();
        private Contract() { }
        public Contract(Guid patientId, ContractNumber contractNumber, ContractPeriod contractPeriod)
        {
            Id = Guid.NewGuid();
            PacienteId = patientId;
            NumeroContrato = contractNumber;
            FechaContrato = DateTime.Now;
            Periodo = contractPeriod;
            Estado = ContractStatus.Draft;
        }
        public Guid PacienteId { get; set; }
        public ContractNumber NumeroContrato { get; set; }
        public DateTime FechaContrato { get; set; }
        public ContractPeriod Periodo {  get; set; }
        public ContractStatus Estado { get; set; }
        public string Observaciones {  get; set; }

        public IReadOnlyCollection<ContractedService> Services => _services.AsReadOnly();
        public AmountValue MontoTotal => new(_services.Sum(s => s.PrecioUnitario.Value));

        #region "Metodos para el Negocio"
        public void AddService(ContractedService service) 
        {
            if (Estado != ContractStatus.Draft)
                throw new DomainException(new Error("AddService","Only draft contracts can be modified.",ErrorType.Validation));
            _services.Add(service);
        }
        public void RemoveService(Guid serviceId)
        {
            if (Estado != ContractStatus.Draft)
                throw new DomainException(new Error("RemoveService", "Only draft contracts can be modified.", ErrorType.Validation));
            var service = _services.FirstOrDefault(x => x.ContractServiceId == serviceId);
            if (service == null)
                throw new DomainException(new Error("RemoveService", "Service not found", ErrorType.Validation));
            _services.Remove(service);
        }
        public void Activate()
        {
            if (_services.Count != 0)
                throw new DomainException(new Error("ContractActivate", "The contract must contrain at least one service", ErrorType.Validation));
            Estado = ContractStatus.Active;
        }
        public void Cancel()
        {
            Estado = ContractStatus.Cancelled;
        }
        #endregion
    }
}
