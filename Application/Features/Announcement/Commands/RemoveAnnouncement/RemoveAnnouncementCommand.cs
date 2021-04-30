using MediatR;


namespace Application.Features.Announcement.Commands.RemoveAnnouncement
{
    public class RemoveAnnouncementCommand : IRequest<RemoveAnnouncementViewModel>
    {
        public int AnnouncementId { get; set; }
    }
}
