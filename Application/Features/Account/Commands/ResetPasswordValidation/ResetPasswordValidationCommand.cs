using MediatR;

namespace Application.Features.Account.Commands.ResetPasswordValidation
{
    public class ResetPasswordValidationCommand : IRequest<Unit>
    {
        public string Email { get; set; }
        public string Token { get; set; }
    }
}