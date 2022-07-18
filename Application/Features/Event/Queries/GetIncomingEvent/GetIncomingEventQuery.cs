using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Event.Queries.GetIncomingEvent;

public class GetIncomingEventQuery : IRequest<GetIncomingEventViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
}