using MediatR;

namespace Application.Features.User.Commands.DeleteUser;

public class DeleteUserCommand : IRequest<Unit>
{
    public string UserId { get; set; }
}