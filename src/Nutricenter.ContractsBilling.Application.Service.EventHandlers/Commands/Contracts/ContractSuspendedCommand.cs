using MediatR;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public sealed record ContractSuspendedCommand(Guid ContractId) : IRequest<Result<bool>>;
}
