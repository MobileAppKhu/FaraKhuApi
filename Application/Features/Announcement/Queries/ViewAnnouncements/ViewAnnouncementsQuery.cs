using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Queries.ViewAnnouncements
{
    public class ViewAnnouncementsQuery : IRequest<ViewAnnouncementsViewModel>
    {
        public string Faculty { get; set; }
        public string Department { get; set; }
    }
}
