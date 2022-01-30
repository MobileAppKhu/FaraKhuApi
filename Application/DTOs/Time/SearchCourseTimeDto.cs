using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Time
{
    public class SearchCourseTimeDto : IMapFrom<Domain.Models.Time>
    {
        public string TimeId { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
        public string CourseEndDate { get; set; }
        public string WeekDay { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Time, SearchCourseTimeDto>()
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