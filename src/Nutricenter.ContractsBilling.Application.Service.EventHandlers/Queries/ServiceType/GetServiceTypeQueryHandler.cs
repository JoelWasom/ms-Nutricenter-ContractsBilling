using MediatR;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using Nutricenter.ContractsBilling.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.Queries.ServiceType
{
    public class GetServiceTypeQueryHandler : IRequestHandler<GetServiceTypesQuery, List<ServiceTypeDTO>>
    {
        private readonly IServiceTypeQuery _repository;

        public GetServiceTypeQueryHandler(IServiceTypeQuery repository)
        {
            _repository = repository;
        }

        public async Task<List<ServiceTypeDTO>> Handle(GetServiceTypesQuery request, CancellationToken cancellationToken)
        {
            var entities = await _repository.GetAllAsync(cancellationToken);

            // Mapear cada entidad a su DTO
            var dtos = entities.Select(s => new ServiceTypeDTO
            {
                ServiceTypeId = s.Id,
                Codigo = s.Codigo,
                Nombre = s.Nombre,
                Descripcion = s.Descripcion,
                IsActive = s.IsActive
            }).ToList();

            return dtos;
        }
    }
}
