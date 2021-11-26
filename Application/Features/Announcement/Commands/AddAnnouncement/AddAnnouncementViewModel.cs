using Application.DTOs.Announcement;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementViewModel
    {
        public SearchAnnouncementDto Announcement { get; set; }
    }
}