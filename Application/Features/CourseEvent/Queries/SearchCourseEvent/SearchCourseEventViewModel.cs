using System.Collections.Generic;
using Application.DTOs.CourseEvent;

namespace Application.Features.CourseEvent.Queries.SearchCourseEvent;

public class SearchCourseEventViewModel
{
    public List<SearchCourseCourseEventDto> CourseEvents { get; set; }
    public int SearchLength { get; set; }
}