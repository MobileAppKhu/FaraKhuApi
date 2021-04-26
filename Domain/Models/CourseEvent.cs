using System;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class CourseEvent
    {
        public int CourseEventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }

        public CourseEventType EventType { get; set; }

        public Course Course { get; set; }

        public int CourseId { get; set; }
    }
}