using MediatR;

namespace Application.Features.Announcement.Commands.EditAnnouncement
{
    public class EditAnnouncementCommand : IRequest<Unit>
    {
        public string AnnouncementId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Department { get; set; }
        public string AvatarId { get; set; }
    }
}