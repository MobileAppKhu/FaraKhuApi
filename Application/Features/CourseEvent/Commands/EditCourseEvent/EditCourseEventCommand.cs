using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.CourseEvent.Commands.EditCourseEvent
{
    public class EditCourseEventCommand : IRequest<Unit>
    {
        public string CourseEventId { get; set; }
        public string EventName{ get; set; }
        public string Description { get; set; }
        public CourseEventType? CourseEventType { get; set; }
        public DateTime? EventTime { get; set; }
    }
}