using MediatR;

namespace Application.Features.User.Command.RemoveUser
{
    public class RemoveUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
    }
}