using MediatR;

namespace Application.Features.Event.Command.UpdateEvent
{
    public class UpdateEventCommand : IRequest<Unit>
    {
        public string EventId { get; set; }
        
        public string EventName { get; set; }
        
        public string EventDescription { get; set; }
        
        public string EventTime { get; set; }
    }
}