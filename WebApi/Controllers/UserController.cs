using System.Threading.Tasks;
using Application.Features.User.Queries.ViewAllEvents;
using Application.Features.User.Queries.ViewProfile;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(ViewAllEventsViewModel),200)]
        public async Task<IActionResult> GetAllEvents(ViewAllEventsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [ProducesResponseType(typeof(ViewProfileViewModel),200)]
        public async Task<IActionResult> ViewProfile(ViewProfileQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}