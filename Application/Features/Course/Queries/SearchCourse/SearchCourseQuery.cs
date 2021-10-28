using MediatR;

namespace Application.Features.Course.Queries.SearchCourse
{
    public class SearchCourseQuery : IRequest<SearchCourseViewModel>
    {
        public string CourseId { get; set; }
    }
}