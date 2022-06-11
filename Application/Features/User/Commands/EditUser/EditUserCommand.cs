using System.Collections.Generic;
using MediatR;

namespace Application.Features.User.Commands.EditUser
{
    public class EditUserCommand : IRequest<Unit>
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string AvatarId { get; set; }
        public bool DeleteAvatar { get; set; }
        public string LinkedIn { get; set; }
        public string GoogleScholar { get; set; }
        public List<string> AddFavourites { get; set; }
        public List<string> DeleteFavourites { get; set; }
    }
}