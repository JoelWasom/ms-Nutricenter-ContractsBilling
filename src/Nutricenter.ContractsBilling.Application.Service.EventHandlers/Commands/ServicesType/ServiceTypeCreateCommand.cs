using MediatR;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType
{
    public record ServiceTypeCreateCommand(string Codigo, string Nombre, bool IsActive = true) : IRequest<Result<Guid>>;
}
