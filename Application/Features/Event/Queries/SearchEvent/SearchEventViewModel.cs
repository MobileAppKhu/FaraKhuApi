using System.Collections.Generic;
using Application.DTOs.Event.PersonalEvent;

namespace Application.Features.Event.Queries.SearchEvent
{
    public class SearchEventViewModel
    {
        public List<EventDto> Event { get; set; }
        public int SearchLength { get; set; }
    }
}