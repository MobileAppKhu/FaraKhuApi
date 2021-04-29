using MediatR;


namespace Application.Features.Announcement.Commands.CreateAnnouncement
{
    public class CreateAnnouncementCommand : IRequest<CreateAnnouncementViewModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
    }
}
