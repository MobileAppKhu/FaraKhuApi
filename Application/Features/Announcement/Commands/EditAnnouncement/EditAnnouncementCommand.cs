using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Announcement.Commands.EditAnnouncement
{
    public class EditAnnouncementCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string AvatarId { get; set; }
    }
}