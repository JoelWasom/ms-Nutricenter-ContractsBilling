using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Nutricenter.BuildingBlocks.Results;
using Nutricenter.ContractsBilling.Application.Service.Commands.Contracts;

namespace Nutricenter.ContractsBilling.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController: ControllerBase
    {
        private readonly IMediator _mediator;
        public InvoiceController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceCreateCommand request)
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
