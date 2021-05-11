using System.Threading.Tasks;
using Application.Features.User.Command.CreateUser;
using Application.Features.User.Command.RemoveUser;
using Application.Features.User.Commands.UpdateAvatar;
using Application.Features.User.Queries.ViewAllEvents;
using Application.Features.User.Queries.ViewProfile;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
        
        [HttpPost]
        [Authorize(Policy = "OwnerPolicy")]

        [ProducesResponseType(typeof(CreateUserViewModel),200)]
        public async Task<IActionResult> CreateUser(CreateUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize(Policy = "OwnerPolicy")]
        public async Task<IActionResult> RemoveUser(RemoveUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> UpdateAvatar(UpdateAvatarCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
    }
}