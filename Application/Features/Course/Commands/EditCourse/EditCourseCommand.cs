using MediatR;

namespace Application.Features.Course.Commands.EditCourse
{
    public class EditCourseCommand : IRequest<Unit>
    {
        public string CourseId { get; set; }
        public string CourseTitle { get; set; }
    }
}