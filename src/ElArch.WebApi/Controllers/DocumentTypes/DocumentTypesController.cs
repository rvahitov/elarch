using System.Threading;
using System.Threading.Tasks;
using ElArch.Domain.Core.Extensions;
using ElArch.WebApi.Controllers.DocumentTypes.Dto;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace ElArch.WebApi.Controllers.DocumentTypes
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class DocumentTypesController : ControllerBase
    {
        private readonly IMediator _mediator;

        public DocumentTypesController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDocumentTypeRequest request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccess()) return Ok(response.AsT0);
            return BadRequest(new {error = response.AsT1.Value});
        }

        [HttpPost("name/change")]
        public async Task<IActionResult> ChangeName(ChangeDocumentTypeNameRequest request, CancellationToken cancellationToken)
        {
            if (ModelState.IsValid == false) return BadRequest(ModelState);
            var response = await _mediator.Send(request, cancellationToken).ConfigureAwait(false);
            if (response.IsSuccess) return NoContent();
            return BadRequest(new {error = response.ToString()});
        }
    }
}