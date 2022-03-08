using MediatR;

namespace Application.Features.Account.Commands.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}