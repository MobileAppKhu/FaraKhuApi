using Application.DTOs.Announcement;
using System.Collections.Generic;

namespace Application.Features.Announcement.Queries.SearchAnnouncements;

public class SearchAnnouncementsViewModel
{
    public List<SearchAnnouncementDto> Announcements{get; set;}
    public int SearchLength { get; set; }
}