using System.Collections.Generic;
using Application.DTOs.CourseType;

namespace Application.Features.CourseType.Queries.SearchCourseType;

public class SearchCourseTypeViewModel
{
    public List<CourseTypeSearchDto> CourseTypes { get; set; }
    public int SearchCount { get; set; }
}