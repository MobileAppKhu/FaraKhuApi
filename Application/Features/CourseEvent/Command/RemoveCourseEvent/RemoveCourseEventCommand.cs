using MediatR;

namespace Application.Features.CourseEvent.Command.RemoveCourseEvent
{
    public class RemoveCourseEventCommand : IRequest<Unit>
    {
        public int CourseId { get; set; }
        public int CourseEventId { get; set; }
    }
}