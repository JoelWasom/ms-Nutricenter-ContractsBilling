using MediatR;
using Nutricenter.BuildingBlocks.Abstractions;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType
{
    public class ServiceTypeUpdateCommandHandler : IRequestHandler<ServiceTypeUpdateCommand, Result<Guid>>
    {
        private readonly IServiceTypeRepository _repository;
        private readonly IUnitOfWork _unitOfWork;

        public ServiceTypeUpdateCommandHandler(
            IServiceTypeRepository repository,
            IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<Guid>> Handle(ServiceTypeUpdateCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id);
            if (entity == null)
                return Result.Failure<Guid>(new Error("ServiceType", "ServiceType not found", ErrorType.Failure));

            // Actualizar campos
            entity.Update(request.Nombre, request.Descripcion, request.NombreMicroservicio, request.BaseUrl, request.Endpoint, request.IsService);

            await _repository.UpdateAsync(entity);
            await _unitOfWork.CommitAsync(cancellationToken);

            return Result.Success(entity.Id);
        }
    }
}
