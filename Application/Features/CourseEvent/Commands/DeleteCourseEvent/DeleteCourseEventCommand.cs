using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.CourseEvent.Commands.DeleteCourseEvent;

public class DeleteCourseEventCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string CourseEventId { get; set; }
}