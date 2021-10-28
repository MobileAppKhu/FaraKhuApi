using System.Collections.Generic;
using Application.DTOs.Course;
using Application.DTOs.User;

namespace Application.Features.Course.Queries.SearchMyCourses
{
    public class SearchMyCoursesViewModel
    {
        public UserCoursesDto Courses { get; set; }
    }
}