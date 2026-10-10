using MediatR;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.DTOs;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public record InvoiceCreateCommand : IRequest<Result<Guid>>
    {
        public Guid PacienteId { get; set; }
        public Guid ContractId { get; set; }
        public string? Observaciones { get; set; }
        public DateOnly FechaVencimiento { get; set; }

    }
}
