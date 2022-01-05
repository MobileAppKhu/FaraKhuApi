using Domain.Enum;
using MediatR;

namespace Application.Features.CourseType.Queries.SearchCourseType
{
    public class SearchCourseTypeQuery : IRequest<SearchCourseTypeViewModel>
    {
        public string CourseTypeId { get; set; }
        public string CourseTypeTitle { get; set; }
        public string CourseTypeCode { get; set; }
        public string DepartmentId { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public CourseTypeColumn CourseTypeColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}