using System;
using Domain.Enum;

namespace Domain.Models
{
    public class Event
    {
        public int EventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }

        public EventType EventType { get; set; }
        
    }
}