using System.Collections.Generic;
using Application.Common.Mappings;
using Application.DTOs.Course;
using AutoMapper;

namespace Application.DTOs.User;

public class UserCoursesDto : IMapFrom<Domain.Models.Instructor>, IMapFrom<Domain.Models.Student>
{
    public ICollection<CourseShortDto> Courses { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Instructor, UserCoursesDto>()
            .ForMember(d => d.Courses, opt
                => opt.MapFrom(src => src.Courses));
        profile.CreateMap<Domain.Models.Student, UserCoursesDto>()
            .ForMember(d => d.Courses, opt
                => opt.MapFrom(src => src.Courses));
    }
}