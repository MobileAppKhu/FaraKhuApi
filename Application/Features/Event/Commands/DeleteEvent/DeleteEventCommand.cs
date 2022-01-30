using MediatR;

namespace Application.Features.Event.Commands.DeleteEvent
{
    public class DeleteEventCommand : IRequest<Unit>
    {
        public string EventId { get; set; }
    }
}