using System;
using Application.Common.Mappings;
using Domain.Enum;
using Domain.Models;

namespace Application.DTOs
{
    public class EventShortDto : IMapFrom<Event>
    {
        public string EventName { get; set; }

        public DateTime EventTime { get; set; }

        public EventType EventType { get; set; }
    }
}