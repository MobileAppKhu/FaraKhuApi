using MediatR;

namespace Application.Features.Course.Command.RemoveCourse
{
    public class RemoveCourseCommand : IRequest<Unit>
    {
        public string CourseId { get; set; }
    }
}