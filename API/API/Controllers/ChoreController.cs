using Application.Services.Chore.Create;
using Application.Services.Chore.Delete;
using Application.Services.Chore.GetMany;
using Application.Services.Chore.GetOne;
using Application.Services.Chore.Update;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("chores")]
    public class ChoreController : BaseController
    {
        private readonly IMediator _mediator;

        public ChoreController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateChoreRequest request)
        {
            var response = await _mediator.Send(request);
            return Ok(response);
        }

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateChoreRequest request)
        {
            await _mediator.Send(request);
            return Ok();
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {
            await _mediator.Send(new DeleteChoreRequest { Id = id });
            return Ok();
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> Get([FromRoute] Guid id)
        {
            var response = await _mediator.Send(new GetOneChoreRequest { Id = id });
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetMany()
        {
            var response = await _mediator.Send(new GetManyChoresRequest());
            return Ok(response);
        }
    }
}
