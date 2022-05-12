using System.Threading.Tasks;
using Application.Features.User.Commands.AddUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Queries.GetUserId;
using Application.Features.User.Queries.SearchAllEvents;
using Application.Features.User.Queries.SearchProfile;
using Application.Features.User.Queries.SearchStudent;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class UserController : ControllerBase
    {
        private readonly IMediator _mediator;
        public UserController(IMediator mediator)
        {
            _mediator = mediator;
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(SearchAllEventsViewModel),200)]
        public async Task<IActionResult> GetAllEvents(SearchAllEventsQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [ProducesResponseType(typeof(SearchProfileViewModel),200)]
        public async Task<IActionResult> SearchProfile(SearchProfileQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize(Policy = "OwnerPolicy")]
        [ProducesResponseType(typeof(AddUserViewModel),200)]
        public async Task<IActionResult> AddUser(AddUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [Authorize(Policy = "OwnerPolicy")]
        public async Task<IActionResult> DeleteUser(DeleteUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(GetUserViewModel),200)]
        public async Task<IActionResult> GetUserId(GetUserIdQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(SearchStudentQueryValidator),200)]
        public async Task<IActionResult> SearchStudent(SearchStudentQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}