using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Course.Queries.SearchCourse
{
    public class SearchCourseQuery : IRequest<SearchCourseViewModel>
    {
        public string CourseId { get; set; }
        public string Instructor { get; set; }
        public string Student { get; set; }
        public string CourseType { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public CourseColumn CourseColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}