using System;
using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.CourseEvent.Commands.EditCourseEvent
{
    public class EditCourseEventCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string CourseEventId { get; set; }
        public string EventName{ get; set; }
        public string Description { get; set; }
        public CourseEventType? CourseEventType { get; set; }
        public string EventTime { get; set; }
    }
}