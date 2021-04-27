using System.Collections.Generic;
using Application.Common.Mappings;
using Application.DTOs.CourseEvent;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Application.DTOs.Time;
using AutoMapper;

namespace Application.DTOs.Course
{
    public class ViewCourseDto : IMapFrom<Domain.Models.Course>
    {
        public string CourseTitle { get; set; }

        public ICollection<ViewCourseTimeDto> Times { get; set; }

        public ICollection<ViewCourseStudentDto> Students { get; set; }

        public ViewCourseInstructorDto Instructor { get; set; }

        public ICollection<ViewCourseCourseEventDto> CourseEvents { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Course, ViewCourseDto>();
        }
    }
}