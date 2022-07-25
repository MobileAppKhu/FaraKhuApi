using System;
using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Event.Commands.AddEvent;

public class AddEventCommand : IRequest<AddEventViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public DateTime EventTime { get; set; }
    public string CourseId { get; set; }
}