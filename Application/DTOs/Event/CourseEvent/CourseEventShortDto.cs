using System;
using Domain.Enum;

namespace Application.DTOs.Event.CourseEvent
{
    public class CourseEventShortDto
    {
        public int EventId { get; set; }
        
        public string EventName { get; set; }

        public DateTime EventTime { get; set; }

        public CourseEventType EventType { get; set; }
    }
}