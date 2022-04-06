using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Announcement.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommand : IRequest<Unit>
    {
        [JsonIgnore]
        public string UserId { get; set; }
        public string AnnouncementId { get; set; }
    }
}
