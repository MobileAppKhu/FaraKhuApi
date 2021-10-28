using Application.DTOs.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class SearchAnnouncementsViewModel
    {
        public ICollection<SearchAnnouncementDto> Announcements{get; set;}
        public int SearchLength { get; set; }
    }
}
