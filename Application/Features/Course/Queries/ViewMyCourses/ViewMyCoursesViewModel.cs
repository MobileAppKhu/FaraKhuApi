using System.Collections.Generic;
using Application.DTOs.Course;
using Application.DTOs.User;

namespace Application.Features.Course.Queries.ViewMyCourses
{
    public class ViewMyCoursesViewModel
    {
        public UserCoursesDto Courses { get; set; }
    }
}