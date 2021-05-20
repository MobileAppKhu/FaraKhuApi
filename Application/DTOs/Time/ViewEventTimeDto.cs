using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Time
{
    public class ViewEventTimeDto : IMapFrom<Domain.Models.Time>
    {
        public string TimeId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseEndDate { get; set; }
        public string InstructorName { get; set; }
        public string CourseTitle { get; set; }
        public string WeekDay { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Time, ViewEventTimeDto>()
                .ForMember(e => e.InstructorName,
                    opt =>
                        opt.MapFrom(src => src.Course.Instructor.FirstName +
                                           " " + src.Course.Instructor.LastName))
                .ForMember(e => e.CourseTitle,
                    opt =>
                        opt.MapFrom(src => src.Course.CourseTitle))
                .ForMember(d => d.StartTime, opt
                    => opt.MapFrom(src => src.StartTime.ToString("h:mm:ss tt zz")))
                .ForMember(d => d.EndTime, opt 
                    => opt.MapFrom(src => src.EndTime.ToString("h:mm:ss tt zz")))
                .ForMember(d => d.CourseEndDate,opt 
                    => opt.MapFrom(src => src.Course.EndDate.ToString("d")))
                .ForMember(src => src.WeekDay, opt 
                    => opt.MapFrom(src => src.WeekDay.ToString()));
        }
    }
}