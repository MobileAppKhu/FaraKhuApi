using MediatR;

namespace Application.Features.Announcement.Queries.ViewInstructorAnnouncements
{
    public class ViewInstructorAnnouncementsQuery : IRequest<ViewInstructorAnnouncementsViewModel>
    {
        public int Start { get; set; }
        public int Step { get; set; }
    }
}