using MediatR;

namespace Application.Features.Announcement.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementCommand : IRequest<Unit>
    {
        public string AnnouncementId { get; set; }
    }
}
