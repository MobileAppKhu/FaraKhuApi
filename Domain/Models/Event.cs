using System;

namespace Domain.Models
{
    public class Event
    {
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }
        
    }
}