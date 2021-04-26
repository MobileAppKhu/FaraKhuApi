using System;
using Application.Common.Mappings;
using Domain.Enum;
using Domain.Models;

namespace Application.DTOs.Event
{
    public class EventShortDto : IMapFrom<Domain.Models.Event>
    {
        public string EventName { get; set; }

        public DateTime EventTime { get; set; }

        public EventType EventType { get; set; }
    }
}