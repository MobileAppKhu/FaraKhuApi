using System.Threading.Tasks;
using Application.Features.Account.SignIn;
using Application.Features.Account.SignOut;
using Application.Features.Account.SignUp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AccountController : ControllerBase
    {
        private IMediator _mediator;
        public AccountController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ProducesResponseType(typeof(SignInViewModel), 200)]
        public async Task<IActionResult> SignIn(SignInCommand request)
        {
            return Ok(await _mediator.Send(request));
        }
        
        [HttpPost]
        public async Task<IActionResult> SignOut(SignOutCommand request)
        {
            await _mediator.Send(request);
            return Ok();
        }
    }
}