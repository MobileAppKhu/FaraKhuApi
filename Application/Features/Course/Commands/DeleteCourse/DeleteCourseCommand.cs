using MediatR;

namespace Application.Features.Course.Commands.DeleteCourse
{
    public class DeleteCourseCommand : IRequest<Unit>
    {
        public string CourseId { get; set; }
    }
}