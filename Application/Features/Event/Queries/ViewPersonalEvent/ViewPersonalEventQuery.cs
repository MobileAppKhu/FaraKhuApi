using MediatR;

namespace Application.Features.Event.Queries.ViewPersonalEvent
{
    public class ViewPersonalEventQuery : IRequest<ViewPersonalEventViewModel>
    {
        public int EventId { get; set; }
    }
}