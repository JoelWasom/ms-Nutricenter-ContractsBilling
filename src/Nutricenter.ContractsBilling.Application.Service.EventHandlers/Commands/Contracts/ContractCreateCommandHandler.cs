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
    public sealed class ContractCreateCommandHandler : IRequestHandler<ContractCreateCommand, Result<Guid>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IContractNumberGenerator _numberGenerator;
        private readonly IUnitOfWork _unitOfWork;
        
        public ContractCreateCommandHandler(IContractRepository contractRepository, IContractNumberGenerator numberGenerator, IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _numberGenerator = numberGenerator;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<Guid>> Handle(ContractCreateCommand request, CancellationToken cancellationToken)
        {
            var contractNumber = await _numberGenerator.GenerateAsync("CTR");

            var period = new ContractPeriod(
                request.FechaInicio,
                request.FechaFin);

            var contract = new Contract(
                request.PacienteId,
                contractNumber,
                period);

            foreach (var item in request.Servicios)
            {
                var reference = new ServiceReference(
                    item.ServiceId,
                    item.Description);

                var amount = new AmountValue(item.UnitPrice, "BOB");

                var service = new ContractedService(
                    item.ServiceTypeId,
                    reference,
                    item.Quantity,
                    amount,
                    item.DurationDays);
                contract.AddService(service);
            }
            await _contractRepository.CreateAsync(contract);
            await _unitOfWork.CommitAsync(cancellationToken);
            return contract.Id;
        }
    }
}
