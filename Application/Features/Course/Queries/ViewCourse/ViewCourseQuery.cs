using MediatR;

namespace Application.Features.Course.Queries.ViewCourse
{
    public class ViewCourseQuery : IRequest<ViewCourseViewModel>
    {
        public int CourseId { get; set; }
    }
}