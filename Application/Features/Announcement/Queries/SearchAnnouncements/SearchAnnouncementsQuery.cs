using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Enum;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class SearchAnnouncementsQuery : IRequest<SearchAnnouncementsViewModel>
    {
        public List<string> AnnouncementIds { get; set; }
        public string Department { get; set; }
        public string Description { get; set; }
        public string Title { get; set; }
        public string User { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public AnnouncementColumn AnnouncementColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}
