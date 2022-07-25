using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Event.Commands.DeleteEvent;

public class DeleteEventCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string EventId { get; set; }
}