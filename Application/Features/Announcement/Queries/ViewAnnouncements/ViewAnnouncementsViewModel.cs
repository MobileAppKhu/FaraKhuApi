using Application.DTOs.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Queries.ViewAnnouncements
{
    public class ViewAnnouncementsViewModel
    {
        public ICollection<ViewAnnouncementDto> Announcements{get; set;}
    }
}
