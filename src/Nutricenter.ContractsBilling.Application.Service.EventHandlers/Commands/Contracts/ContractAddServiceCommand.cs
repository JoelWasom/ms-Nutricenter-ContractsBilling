using MediatR;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using System.Collections;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public record ContractAddServiceCommand(
        Guid ContractId,
        Guid ServiceTypeId,
        string ServiceId,
        string Description,
        decimal UnitPrice,
        int Quantity,
        int DurationDays
        ) : IRequest<Result<Guid>>;
    
}
