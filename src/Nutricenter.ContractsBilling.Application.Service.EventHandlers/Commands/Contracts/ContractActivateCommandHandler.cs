using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public sealed class ContractActivateCommandHandler : IRequestHandler<ContractActivateCommand, Result<bool>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContractActivateCommandHandler(IContractRepository contractRepository, IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(ContractActivateCommand request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetByIdAsync(request.ContractId) ??
                throw new DomainException(new Error("ActivateContract", "Contrato no encontrado.", ErrorType.Validation));

            contract.Activate();
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
