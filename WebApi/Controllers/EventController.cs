using System.Threading.Tasks;
using Application.Features.Event.Command.CreateEvent;
using Application.Features.Event.Command.DeleteEvent;
using Application.Features.Event.Command.UpdateEvent;
using Application.Features.Event.Queries.ViewPersonalEvent;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class EventController : ControllerBase
    {
        private IMediator _mediator;
        public EventController(IMediator mediator)
        {
            _mediator = mediator;
        }
        [HttpPost]
        [ProducesResponseType(typeof(CreateEventViewModel),200)]
        public async Task<IActionResult> CreateEvent(CreateEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(DeleteEventViewModel),200)]
        public async Task<IActionResult> DeleteEvent(DeleteEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(UpdateEventViewModel),200)]
        public async Task<IActionResult> UpdateEvent(UpdateEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ViewPersonalEventViewModel),200)]
        public async Task<IActionResult> UpdateEvent(ViewPersonalEventQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
    }
    
}