using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Event.PersonalEvent;

public class EventShortDto : IMapFrom<Domain.Models.Event>
{
    public string EventId { get; set; }
    public string EventName { get; set; }
    public DateTime EventTime { get; set; }
    public string EventDescription { get; set; }
    public bool IsDone { get; set; }
    public string CourseId { get; set; }
    public string CourseTitle { get; set; }
    public DateTime CreatedDate { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Event, EventShortDto>()
            .ForMember(e => e.EventDescription,
                opt =>
                    opt.MapFrom(src => src.EventDescription));
    }
}