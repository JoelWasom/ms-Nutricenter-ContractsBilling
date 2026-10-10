using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.Contracts;
using Nutricenter.ContractsBilling.Application.Service.Commands.ServicesType;

namespace Nutricenter.ContractsBilling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContractController: ControllerBase
    {
        private readonly IMediator _mediator;
        public ContractController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateContrat([FromBody] ContractCreateCommand request)
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
        [HttpPost("{id:guid}/services")]
        public async Task<IActionResult> AddService([FromBody] ContractAddServiceCommand request)
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
        [HttpDelete("{id:guid}/services/{serviceId:guid}")]
        public async Task<IActionResult> RemoveService([FromBody] ContractRemoveServiceCommand request)
        {
            var result = await _mediator.Send(request);
            if (result.IsFailure)
            {
                return result.Error.Type == ErrorType.NotFound ?
                                NotFound(result.Error.Description) :
                                BadRequest(result.Error);
            }
            return NoContent();
        }

        [HttpPatch("{id:guid}/activate")]
        public async Task<IActionResult> Activate([FromBody] ContractActivateCommand request)
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
        [HttpPatch("{id:guid}/suspend")]
        public async Task<IActionResult> Suspend([FromBody] ContractSuspendedCommand request)
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
        [HttpPatch("{id:guid}/cancel")]
        public async Task<IActionResult> Cancel([FromBody] ContractCancelCommand request)
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
        [HttpPatch("{id:guid}/complete")]
        public async Task<IActionResult> Complete([FromBody] ContractTerminatedCommand request)
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
    }
}
