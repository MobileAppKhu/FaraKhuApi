﻿using System;
using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models
{
    public class Event : BaseEntity
    {
        public int EventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }

        public EventType EventType { get; set; }

        public BaseUser User { get; set; }

        public string UserId { get; set; }
    }
}