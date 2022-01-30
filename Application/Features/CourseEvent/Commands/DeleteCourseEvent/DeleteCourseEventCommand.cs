using MediatR;

namespace Application.Features.CourseEvent.Commands.DeleteCourseEvent
{
    public class DeleteCourseEventCommand : IRequest<Unit>
    {
        public string CourseEventId { get; set; }
    }
}