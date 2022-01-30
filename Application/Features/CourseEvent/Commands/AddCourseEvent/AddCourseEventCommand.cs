using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.CourseEvent.Commands.AddCourseEvent
{
    public class AddCourseEventCommand : IRequest<AddCourseEventViewModel>
    {
    public string EventName { get; set; }
    public string EventDescription { get; set; }
    public DateTime EventTime { get; set; }
    public CourseEventType EventType { get; set; }
    public string CourseId { get; set; }
    }
}