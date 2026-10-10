using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public class ContractTerminatedCommandHandler : IRequestHandler<ContractTerminatedCommand, Result<bool>>
    {
        private readonly IContractRepository _contractRepository;
        private readonly IUnitOfWork _unitOfWork;
        public ContractTerminatedCommandHandler(IContractRepository contractRepository, IUnitOfWork unitOfWork)
        {
            _contractRepository = contractRepository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<bool>> Handle(ContractTerminatedCommand request, CancellationToken cancellationToken)
        {
            var contract = await _contractRepository.GetByIdAsync(request.ContractId) ??
                throw new DomainException(new Error("TerminatedContract", "Contrato no encontrado.", ErrorType.Validation));

            contract.Terminated();
            await _unitOfWork.CommitAsync(cancellationToken);
            return true;
        }
    }
}
