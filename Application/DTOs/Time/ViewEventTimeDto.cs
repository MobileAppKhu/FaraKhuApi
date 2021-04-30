using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Time
{
    public class ViewEventTimeDto : IMapFrom<Domain.Models.Time>
    {
        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public string InstructorName { get; set; }

        public string CourseTitle { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Time, ViewEventTimeDto>()
                .ForMember(e => e.InstructorName,
                    opt =>
                        opt.MapFrom(src => src.Course.Instructor.FirstName +
                                           " " + src.Course.Instructor.LastName))
                .ForMember(e => e.CourseTitle,
                    opt =>
                        opt.MapFrom(src => src.Course.CourseTitle));
        }
    }
}