using MediatR;

namespace Application.Features.Account.ForgetPassword
{
    public class ForgetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
    }
}