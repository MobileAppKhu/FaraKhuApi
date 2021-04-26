using System.Collections.Generic;
using System.Linq;
using Application.Common.Mappings;
using Application.DTOs.Event;
using AutoMapper;
using Domain.Models;

namespace Application.DTOs.User
{
    public class AllEventsDto : IMapFrom<Domain.Models.Student>, IMapFrom<Domain.Models.Instructor>
    {
        public ICollection<EventShortDto> Events { get; set; }
        public ICollection<Time> Times { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Student, AllEventsDto>()
                .ForMember(e => e.Events,
                    opt =>
                        opt.MapFrom(src => src.Events))
                .ForMember(e => e.Times,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.Times)));//TODO
            
            profile.CreateMap<Domain.Models.Instructor, AllEventsDto>()
                .ForMember(e => e.Events,
                    opt =>
                        opt.MapFrom(src => src.Events))
                .ForMember(e => e.Times,
                    opt =>
                        opt.MapFrom(src => src.Courses.SelectMany(c => c.Times)));//TODO
        }
    }
}