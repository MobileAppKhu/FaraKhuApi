using System;
using MediatR;

namespace Application.Features.User.Command.UpdateProfile
{
    public class UpdateProfileCommand : IRequest<Unit>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}