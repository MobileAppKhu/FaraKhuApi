using MediatR;

namespace Application.Features.Account.EmailVerification
{
    public class EmailVerificationCommand : IRequest<EmailVerificationViewModel>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}