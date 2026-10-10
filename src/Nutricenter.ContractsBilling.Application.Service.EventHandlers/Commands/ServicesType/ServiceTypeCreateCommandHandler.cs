using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Entities;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType
{
    public class ServiceTypeCreateCommandHandler: IRequestHandler<ServiceTypeCreateCommand, Result<Guid>>
    {
        private readonly IServiceTypeRepository _serviceTypeRepository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypeCreateCommandHandler(IServiceTypeRepository serviceTypeRepository, IUnitOfWork unitOfWork)
        {
            _serviceTypeRepository = serviceTypeRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(ServiceTypeCreateCommand request, CancellationToken cancellationToken)
        {
            var obj = new ServiceType(request.Codigo, request.Nombre, request.IsActive);

            await _serviceTypeRepository.CreateAsync(obj);

            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(obj.Id);

        }
    }
}
