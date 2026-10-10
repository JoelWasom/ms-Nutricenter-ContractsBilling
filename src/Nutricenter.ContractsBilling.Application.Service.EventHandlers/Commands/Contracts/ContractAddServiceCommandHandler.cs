using MediatR;
using Microsoft.EntityFrameworkCore;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.ExternalServices;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using Nutricenter.ContractsBilling.Domain.Shared;
using Nutricenter.ContractsBilling.Domain.ValueObjects;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public sealed class ContractAddServiceCommandHandler: IRequestHandler<ContractAddServiceCommand, Result<Guid>>
    {
        private readonly IServiceCatalog _catalog;
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContractAddServiceCommandHandler(IServiceCatalog catalog, IContractRepository contractRepository, IUnitOfWork unitOfWork)
        {
            _catalog = catalog;
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(ContractAddServiceCommand request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetByIdAsync(request.ContractId) ??
                throw new DomainException(new Error("AddContractService", "Contrato no encontrado.", ErrorType.Validation));

            var contractedService = new ContractedService(
                request.ServiceTypeId,
                new ServiceReference(request.ServiceId, request.Description),
                new QuantityValue(request.Quantity),
                new AmountValue(request.UnitPrice),
                new QuantityValue(request.DurationDays));

            contractedService.ContractId = request.ContractId;

            contract.AddService(contractedService);
            try
            {
                await _unitOfWork.CommitAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                // Aquí puedes inspeccionar qué entidad falló
                foreach (var entry in ex.Entries)
                {
                    var entityName = entry.Entity.GetType().Name;
                    Console.WriteLine($"Concurrencia falló en: {entityName}");
                }
                throw;
            }
            return contractedService.Id;
        }
    }
}
