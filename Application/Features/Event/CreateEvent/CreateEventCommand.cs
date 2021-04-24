﻿using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Event.CreateEvent
{
    public class CreateEventCommand : IRequest<CreateEventViewModel>
    {
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }

        public EventType EventType { get; set; }
    }
}
/*{
    "EventName" : "Quiz-1",
    "EventDescription" : "Quiz Instructor 2",
    "EventTime":"21-4-24",
    "EventType" : 1
}*/