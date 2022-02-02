using System.Threading.Tasks;
using Application.Features.Event.Commands.AddEvent;
using Application.Features.Event.Commands.DeleteEvent;
using Application.Features.Event.Commands.EditEvent;
using Application.Features.Event.Queries.GetIncomingEvent;
using Application.Features.Event.Queries.SearchEvent;
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
        [ProducesResponseType(typeof(AddEventViewModel),200)]
        public async Task<IActionResult> AddEvent(AddEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> DeleteEvent(DeleteEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> EditEvent(EditEventCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SearchEventViewModel),200)]
        public async Task<IActionResult> SearchEvent(SearchEventQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(GetIncomingEventViewModel),200)]
        public async Task<IActionResult> GetIncomingEvents(GetIncomingEventQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}