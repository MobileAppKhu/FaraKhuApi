using MediatR;

namespace Application.Features.Event.Commands.EditEvent
{
    public class EditEventCommand : IRequest<Unit>
    {
        public string EventId { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventTime { get; set; }
        public bool changingIsDone { get; set; }
    }
}