using MediatR;

namespace Application.Features.Course.Queries.ViewCourse
{
    public class ViewCourseQuery : IRequest<ViewCourseViewModel>
    {
        public string CourseId { get; set; }
    }
}