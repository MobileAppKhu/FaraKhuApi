using System.Collections.Generic;
using Application.Common.Mappings;
using Application.DTOs.CourseEvent;
using Application.DTOs.Instructor;
using Application.DTOs.Student;
using Application.DTOs.Time;
using AutoMapper;

namespace Application.DTOs.Course
{
    public class SearchCourseDto : IMapFrom<Domain.Models.Course>
    {
        public string CourseId { get; set; }
        public string CourseType { get; set; }
        public string Department { get; set; }
        public string Faculty { get; set; }

        public ICollection<SearchCourseTimeDto> Times { get; set; }

        public ICollection<SearchCourseStudentDto> Students { get; set; }

        public SearchCourseInstructorDto Instructor { get; set; }

        public ICollection<SearchCourseCourseEventDto> CourseEvents { get; set; }
        public string AvatarId { get; set; }
        public int AvailablePolls { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Course, SearchCourseDto>()
                .ForMember(d => d.AvailablePolls,
                    opt =>
                        opt.MapFrom(src => src.Polls.Count))
                .ForMember(d => d.Faculty,
                    opt =>
                        opt.MapFrom(src => src.CourseType.Department.Faculty.FacultyTitle))
                .ForMember(d => d.Department,
                    opt =>
                        opt.MapFrom(src => src.CourseType.Department.DepartmentTitle))
                .ForMember(d => d.CourseType,
                    opt =>
                        opt.MapFrom(src => src.CourseType.CourseTypeTitle));
        }
    }
}