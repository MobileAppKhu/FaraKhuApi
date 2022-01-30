using Domain.Enum;
using MediatR;

namespace Application.Features.User.Commands.AddUser
{
    public class AddUserCommand : IRequest<AddUserViewModel>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public UserType UserType { get; set; }
        public string Id { get; set; }
        public string Password { get; set; }
        public string LinkedIn { get; set; }
        public string GoogleScholar { get; set; }
    }
}