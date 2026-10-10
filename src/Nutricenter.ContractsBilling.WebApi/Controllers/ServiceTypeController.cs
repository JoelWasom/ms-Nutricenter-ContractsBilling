using MediatR;
using Microsoft.AspNetCore.Mvc;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType;
using Nutricenter.ContractsBilling.Application.Service.Queries;
using Nutricenter.ContractsBilling.Application.Service.Queries.ServiceType;

namespace Nutricenter.ContractsBilling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServiceTypeController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ServiceTypeController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateServiceType([FromBody] ServiceTypeCreateCommand request)
        {
            var result = await _mediator.Send(request);

            if (result.IsFailure)
            {
                return result.Error.Type == ErrorType.NotFound ?
                                NotFound(result.Error.Description) :
                                BadRequest(result.Error);
            }

            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var query = new GetServiceTypesQuery();
            var result = await _mediator.Send(query);

            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] ServiceTypeUpdateCommand command)
        {
            // Validar que el id de la ruta coincida con el del body
            if (id != command.Id)
                return BadRequest("El Id de la ruta no coincide con el del body.");

            var result = await _mediator.Send(command);

            if (!result.IsSuccess)
                return NotFound(result.Error);

            return Ok(result.Value); // Devuelves el Guid actualizado

        }
    }
}