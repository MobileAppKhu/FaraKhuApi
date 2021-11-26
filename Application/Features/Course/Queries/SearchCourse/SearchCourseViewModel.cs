using System.Collections.Generic;
using Application.DTOs.Course;

namespace Application.Features.Course.Queries.SearchCourse
{
    public class SearchCourseViewModel
    {
        public List<SearchCourseDto> Course { get; set; }
        public int SearchLength { get; set; }
    }
}