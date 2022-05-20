using System.Threading.Tasks;
using Application.Features.User.Commands.AddUser;
using Application.Features.User.Commands.DeleteUser;
using Application.Features.User.Commands.EditUser;
using Application.Features.User.Queries.GetUserId;
using Application.Features.User.Queries.SearchAllEvents;
using Application.Features.User.Queries.SearchProfile;
using Application.Features.User.Queries.SearchStudent;
using Application.Features.User.Queries.SearchUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

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
        [Authorize]
        [ProducesResponseType(typeof(SearchAllEventsViewModel),200)]
        public async Task<IActionResult> GetAllEvents(SearchAllEventsQuery request)
        {
            request.UserId = this.GetUserId();
            return Ok(await _mediator.Send(request));
        }
        [HttpPost]
        [Authorize]
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
        [Authorize(Policy = "OwnerPolicy")]
        public async Task<IActionResult> EditUser(EditUserCommand request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(GetUserViewModel),200)]
        public async Task<IActionResult> GetUserId(GetUserIdQuery request)
        {
            return Ok(new GetUserViewModel
            {
                UserId = this.GetUserId()
            });
        }

        [HttpPost]
        [Authorize(Policy = "InstructorPolicy")]
        [ProducesResponseType(typeof(SearchStudentQueryValidator),200)]
        public async Task<IActionResult> SearchStudent(SearchStudentQuery request)
        {
            return Ok(await _mediator.Send(request));
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(typeof(SearchUserViewModel),200)]
        public async Task<IActionResult> SearchUser(SearchUserQuery request)
        {
            return Ok(await _mediator.Send(request));
        }
    }
}