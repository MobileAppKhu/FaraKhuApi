using System;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Event : BaseEntity
    {
        public string EventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }
        
        public BaseUser User { get; set; }

        public string UserId { get; set; }
        public bool isDone { get; set; }
    }
}