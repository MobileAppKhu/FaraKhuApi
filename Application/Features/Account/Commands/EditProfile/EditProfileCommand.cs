using System.Collections.Generic;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Account.Commands.EditProfile
{
    public class EditProfileCommand : IRequest<Unit>
    {
        [JsonIgnore]
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