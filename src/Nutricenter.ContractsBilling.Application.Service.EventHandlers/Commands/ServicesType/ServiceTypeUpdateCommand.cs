using MediatR;
using Nutricenter.BuildingBlocks.Results;
using System;
using System.Collections.Generic;
using System.Text;

namespace Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType
{
    public record ServiceTypeUpdateCommand(
        Guid Id,
        string Nombre,
        string? Descripcion,
        string? NombreMicroservicio,
        string? BaseUrl,
        string? Endpoint,
        bool IsService,
        bool IsActive
        ) : IRequest<Result<Guid>>;
}
