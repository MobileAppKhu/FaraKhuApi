using System;
using Domain.Enum;

namespace Domain.Models
{
    public class CourseEvent : BaseEntity
    {
        public string CourseEventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }

        public CourseEventType EventType { get; set; }

        public Course Course { get; set; }

        public string CourseId { get; set; }
    }
}