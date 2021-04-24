using System.Threading.Tasks;
using Application.Features.Event.CreateEvent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    
    [Authorize]
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
        
    }
    
}