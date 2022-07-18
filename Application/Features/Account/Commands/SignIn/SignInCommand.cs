using MediatR;

namespace Application.Features.Account.Commands.SignIn;

public class SignInCommand : IRequest<SignInViewModel>
{
    public string Logon { get; set; }
    public string Password { get; set; }
}