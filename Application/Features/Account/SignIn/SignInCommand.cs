using MediatR;

namespace Application.Features.Account.SignIn
{
    public class SignInCommand : IRequest<SignInViewModel>
    {
        public string Logon { get; set; }
        public string Password { get; set; }
    }
}