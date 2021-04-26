using MediatR;

namespace Application.Features.Event.Command.DeleteEvent
{
    public class DeleteEventCommand : IRequest<DeleteEventViewModel>
    {
        public int EventId { get; set; }
    }
}