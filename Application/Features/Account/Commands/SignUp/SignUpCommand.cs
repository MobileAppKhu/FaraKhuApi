using Domain.Enum;
using MediatR;

namespace Application.Features.Account.SignUp
{
    public class SignUpCommand : IRequest<SignUpViewModel>
    {
        public string FirstName { get; set; }
        
        public string LastName { get; set; }

        public string Email { get; set; }

        public UserType UserType { get; set; }

        public string Id { get; set; }

        public string Password { get; set; }
    }
}