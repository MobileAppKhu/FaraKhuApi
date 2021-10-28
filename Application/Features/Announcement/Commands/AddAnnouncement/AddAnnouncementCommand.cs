using MediatR;


namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementCommand : IRequest<AddAnnouncementViewModel>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Faculty { get; set; }
        public string Department { get; set; }
    }
}
