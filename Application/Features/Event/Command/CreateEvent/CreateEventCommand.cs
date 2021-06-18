using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Event.Command.CreateEvent
{
    public class CreateEventCommand : IRequest<CreateEventViewModel>
    {
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }
        
    }
}
