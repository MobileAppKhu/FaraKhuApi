using MediatR;

namespace Application.Features.Announcement.Queries.SearchInstructorAnnouncements
{
    public class SearchInstructorAnnouncementsQuery : IRequest<SearchInstructorAnnouncementsViewModel>
    {
        public int Start { get; set; }
        public int Step { get; set; }
    }
}