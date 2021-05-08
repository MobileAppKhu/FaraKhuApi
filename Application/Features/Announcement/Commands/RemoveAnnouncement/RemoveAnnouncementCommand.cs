using MediatR;


namespace Application.Features.Announcement.Commands.RemoveAnnouncement
{
    public class RemoveAnnouncementCommand : IRequest<RemoveAnnouncementViewModel>
    {
        public string AnnouncementId { get; set; }
    }
}
