using MediatR;

namespace Application.Features.Event.Command.DeleteEvent
{
    public class DeleteEventCommand : IRequest<Unit>
    {
        public string EventId { get; set; }
    }
}