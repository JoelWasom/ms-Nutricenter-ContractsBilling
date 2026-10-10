using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Enums;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Domain.Aggregate
{
    public class Invoice : AggregateRoot
    {
        private readonly List<InvoiceDetail> _detail = new();
        private readonly List<Payment> _pagos = new();
        private Invoice() { }
        public Invoice(Guid patientId, Guid contractId, SequenceNumberGenerator invoiceNumber, DateOnly fechaVencimiento)
        {
            Id = Guid.NewGuid();
            PacienteId = patientId;
            ContractId = contractId;
            NumeroFactura = invoiceNumber;
            FechaEmision = DateTime.Now;
            FechaVencimiento = fechaVencimiento;
            Estado = InvoiceStatus.Pending;
        }
        public Guid PacienteId { get; set; }
        public Guid? ContractId { get; set; }
        public SequenceNumberGenerator NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }
        public DateOnly FechaVencimiento { get; set; }
        //public AmountValue SaldoPendiente { get; set; }
        public InvoiceStatus Estado { get; set; }
        public string? Observaciones { get; set; }

        public IReadOnlyCollection<InvoiceDetail> Services => _detail.AsReadOnly();
        public IReadOnlyCollection<Payment> Pagos => _pagos.AsReadOnly();
        public AmountValue MontoTotal => new(_detail.Sum(s => s.PrecioUnitario.Value));
        //public AmountValue SaldoPendiente { get; set; }
        #region "Metodos para el Negocio"
        public void AddDetail(InvoiceDetail detail)
        {
            if (_detail.Any(x => x.ServiceReferencia == detail.ServiceReferencia))
                throw new DomainException(new Error("AddService", "The service already exists in the contract.", ErrorType.Validation));
            _detail.Add(detail);
        }
        public void RemoveDetail(Guid InvoiceDetailId)
        {
            //if (Estado != ContractStatus.Draft)
            //    throw new DomainException(new Error("RemoveService", "Only draft contracts can be modified.", ErrorType.Validation));
            var detail = _detail.FirstOrDefault(x => x.Id == InvoiceDetailId) ?? throw new DomainException(new Error("RemoveService", "Service not found", ErrorType.Validation));
            _detail.Remove(detail);
        }
        public void Expired()
        {
            if (Estado != InvoiceStatus.Partial)
                throw new DomainException(new Error("InvoiceExpired", "Only invoices with partial payments can be declared overdue.", ErrorType.Validation));
            if (_detail.Count != 0)
                throw new DomainException(new Error("InvoiceExpired", "The invoice must include at least one service.", ErrorType.Validation));
            Estado = InvoiceStatus.Expired;
        }
        public void Cancelled()
        {
            Estado = InvoiceStatus.Cancelled; /*ó Anulado*/
        }
        public void PaymentCompleted()
        {
            if (Estado != InvoiceStatus.Pending || Estado != InvoiceStatus.Partial)
                throw new DomainException(new Error("InvoicePaiment", "Only payments for outstanding and partial invoices can be completed.", ErrorType.Validation));

            Estado = InvoiceStatus.Paid;
        }
        #endregion
    }
}
