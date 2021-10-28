using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Event.Commands.AddEvent
{
    public class AddEventCommand : IRequest<AddEventViewModel>
    {
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public DateTime EventTime { get; set; }
        
    }
}
