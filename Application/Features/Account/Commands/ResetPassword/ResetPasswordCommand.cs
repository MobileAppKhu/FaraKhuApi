using MediatR;

namespace Application.Features.Account.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string NewPassword { get; set; }
    }
}