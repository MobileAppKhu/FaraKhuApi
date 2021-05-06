using MediatR;

namespace Application.Features.Event.Command.DeleteEvent
{
    public class DeleteEventCommand : IRequest<Unit>
    {
        public int EventId { get; set; }
    }
}