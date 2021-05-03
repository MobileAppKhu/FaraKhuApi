using Domain.Enum;
using MediatR;

namespace Application.Features.User.Command.CreateUser
{
    public class CreateUserCommand : CreateUserViewModel, IRequest<CreateUserViewModel>
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public string Id { get; set; }

        public string Password { get; set; }
    }
}