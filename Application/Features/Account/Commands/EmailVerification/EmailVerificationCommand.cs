using MediatR;

namespace Application.Features.Account.Commands.EmailVerification;

public class EmailVerificationCommand : IRequest<EmailVerificationViewModel>
{
    public string Email { get; set; }
    public string Token { get; set; }
}