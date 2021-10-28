using System.Collections;
using System.Collections.Generic;
using Application.DTOs.Announcement;

namespace Application.Features.Announcement.Queries.SearchInstructorAnnouncements
{
    public class SearchInstructorAnnouncementsViewModel
    {
        public ICollection<SearchAnnouncementDto> Announcements { get; set; }
        public int SearchLength { get; set; }
    }
}