using Domain.Enum;
using MediatR;

namespace Application.Features.Event.Command.CreateEvent
{
    public class CreateEventCommand : IRequest<CreateEventViewModel>
    {
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public string EventTime { get; set; }
        
    }
}
/*{
    "EventName" : "Quiz-1",
    "EventDescription" : "Quiz Instructor 2",
    "EventTime":"21-4-24",
    "EventType" : 1
}*/