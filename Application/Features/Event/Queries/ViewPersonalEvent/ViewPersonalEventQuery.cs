using MediatR;

namespace Application.Features.Event.Queries.ViewPersonalEvent
{
    public class ViewPersonalEventQuery : IRequest<ViewPersonalEventViewModel>
    {
        public string EventId { get; set; }
    }
}