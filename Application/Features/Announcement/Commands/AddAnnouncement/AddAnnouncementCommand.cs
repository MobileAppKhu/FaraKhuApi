using System.Text.Json.Serialization;
using MediatR;


namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementCommand : IRequest<AddAnnouncementViewModel>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string Avatar { get; set; }
    }
}
