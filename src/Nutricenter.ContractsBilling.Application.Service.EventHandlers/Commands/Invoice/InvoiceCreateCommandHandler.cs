using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Aggregate;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.Interfaces.Services;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public sealed class InvoiceCreateCommandHandler : IRequestHandler<InvoiceCreateCommand, Result<Guid>>
    {
        private readonly IInvoicetRepository _invoiceRepository;
        private readonly IContractRepository _contractRepository;
        private readonly IInvoiceNumberGenerator _numberGenerator;
        private readonly IUnitOfWork _unitOfWork;
        
        public InvoiceCreateCommandHandler(IInvoicetRepository invoiceRepository, IContractRepository contractRepository, IInvoiceNumberGenerator numberGenerator, IUnitOfWork unitOfWork)
        {
            _invoiceRepository = invoiceRepository;
            _contractRepository = contractRepository;
            _numberGenerator = numberGenerator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(InvoiceCreateCommand request, CancellationToken cancellationToken)
        {
            var invoiceNumber = await _numberGenerator.GenerateAsync("FAC");
            var contract = await _contractRepository.GetByIdAsync(request.ContractId) ??
                throw new DomainException(new Error("CancelContract", "Contrato no encontrado.", ErrorType.Validation));
            var invoice = new Invoice(
                request.PacienteId,
                request.ContractId,
                invoiceNumber,
                request.FechaVencimiento);

            foreach (var item in contract.Services)
            {
                var reference = new ServiceReference(
                    item.ServiceReferencia.ServiceId,
                    item.ServiceReferencia.Descripcion);
                var detail = new InvoiceDetail(
                    reference,
                    item.Cantidad,
                    item.PrecioUnitario);
                invoice.AddDetail(detail);
            }
            await _invoiceRepository.CreateAsync(invoice);
            await _unitOfWork.CommitAsync(cancellationToken);
            return invoice.Id;
        }
    }
}
