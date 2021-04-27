using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using Application.DTOs.Event.CourseEvent;
using Application.DTOs.Event.PersonalEvent;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.User
{
    public class AllEventsDto : IMapFrom<Domain.Models.Student>, IMapFrom<Domain.Models.Instructor>
    {
        public ICollection<EventShortDto> Events { get; set; }
        public ICollection<CourseEventShortDto> CourseEvents { get; set; }
        public ICollection<Domain.Models.Time> Times { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Student, AllEventsDto>()
                .ForMember(e => e.Events,
                    opt =>
                        opt.MapFrom(src => src.Events))
                .ForMember(e => e.Times,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.Times)))
                .ForMember(e => e.CourseEvents,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.CourseEvents)));//TODO
            
            profile.CreateMap<Domain.Models.Instructor, AllEventsDto>()
                .ForMember(e => e.Events,
                    opt =>
                        opt.MapFrom(src => src.Events))
                .ForMember(e => e.Times,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.Times)))
                .ForMember(e => e.CourseEvents,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.CourseEvents)));//TODO
        }
    }
}