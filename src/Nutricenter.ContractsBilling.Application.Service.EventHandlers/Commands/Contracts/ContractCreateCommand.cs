using MediatR;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.Contracts
{
    public record ContractCreateCommand : IRequest<Result<Guid>>
    {
        public Guid PacienteId { get; set; }
        public string? Observaciones { get; set; }
        public DateOnly FechaInicio { get; set; }
        public DateOnly FechaFin {  get; set; }
        public required ICollection<ContractServiceCreateDTO> Servicios { get; set; }

    }
}
