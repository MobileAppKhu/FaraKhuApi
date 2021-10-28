using MediatR;

namespace Application.Features.User.Commands.RemoveUser
{
    public class RemoveUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
    }
}