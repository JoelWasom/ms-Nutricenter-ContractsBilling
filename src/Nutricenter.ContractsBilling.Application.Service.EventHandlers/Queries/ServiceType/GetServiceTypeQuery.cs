using MediatR;
using Nutricenter.ContractsBilling.Application.Service.DTOs;

namespace Nutricenter.ContractsBilling.Application.Service.Queries.ServiceType
{
    public record GetServiceTypesQuery() : IRequest<List<ServiceTypeDTO>>;
}
