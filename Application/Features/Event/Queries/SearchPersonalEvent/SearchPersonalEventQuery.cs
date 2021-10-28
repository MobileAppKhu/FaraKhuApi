using MediatR;

namespace Application.Features.Event.Queries.SearchPersonalEvent
{
    public class SearchPersonalEventQuery : IRequest<SearchPersonalEventViewModel>
    {
        public string EventId { get; set; }
    }
}