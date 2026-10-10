using MediatR;
using Nutricenter.BuildingBlocks.Results;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public sealed record ContractRemoveServiceCommand(Guid ContractId, Guid ContractedServiceId):IRequest<Result<bool>>;
}
