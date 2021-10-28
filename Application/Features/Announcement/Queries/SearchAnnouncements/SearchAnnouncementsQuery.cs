using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Queries.SearchAnnouncements
{
    public class SearchAnnouncementsQuery : IRequest<SearchAnnouncementsViewModel>
    {
        public string Faculty { get; set; }
        public string Department { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
    }
}
