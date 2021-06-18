using System.Collections;
using System.Collections.Generic;
using Application.DTOs.Announcement;

namespace Application.Features.Announcement.Queries.ViewInstructorAnnouncements
{
    public class ViewInstructorAnnouncementsViewModel
    {
        public ICollection<ViewAnnouncementDto> Announcements { get; set; }
        public int SearchLength { get; set; }
    }
}