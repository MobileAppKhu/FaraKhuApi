using System;
using MediatR;

namespace Application.Features.User.Commands.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Unit>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}