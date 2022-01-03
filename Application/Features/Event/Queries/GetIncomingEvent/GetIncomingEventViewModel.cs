using Application.DTOs.User;

namespace Application.Features.Event.Queries.GetIncomingEvent
{
    public class GetIncomingEventViewModel
    {
        public IncomingEventDto Events { get; set; }
        public int Count { get; set; }
    }
}