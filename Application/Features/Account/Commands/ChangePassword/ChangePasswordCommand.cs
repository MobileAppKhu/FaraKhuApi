using MediatR;

namespace Application.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Unit>
    {
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
    }
}