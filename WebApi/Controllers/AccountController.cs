using System.Threading.Tasks;
using Application.Features.Account.Commands.ChangePassword;
using Application.Features.Account.Commands.EditProfile;
using Application.Features.Account.Commands.EmailVerification;
using Application.Features.Account.Commands.ForgetPassword;
using Application.Features.Account.Commands.ResetPassword;
using Application.Features.Account.Commands.ResetPasswordValidation;
using Application.Features.Account.Commands.SignIn;
using Application.Features.Account.Commands.SignOut;
using Application.Features.Account.Commands.SignUp;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApi.Utilities;

namespace WebApi.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]/[action]")]
public class AccountController : ControllerBase
{
    private readonly IMediator _mediator;
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
    [ProducesResponseType(typeof(EmailVerificationViewModel), 200)]
    public async Task<IActionResult> EmailVerify(EmailVerificationCommand request)
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
        return Ok(await _mediator.Send(request));
    }
        
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ForgetPassword(ForgetPasswordCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPasswordValidation(ResetPasswordValidationCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> ResetPassword(ResetPasswordCommand request)
    {
        return Ok(await _mediator.Send(request));
    }
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }

    [HttpPost]
    public async Task<IActionResult> EditProfile(EditProfileCommand request)
    {
        request.UserId = this.GetUserId();
        return Ok(await _mediator.Send(request));
    }
}