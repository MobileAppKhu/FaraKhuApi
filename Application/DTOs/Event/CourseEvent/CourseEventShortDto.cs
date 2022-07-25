using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.Event.CourseEvent;

public class CourseEventShortDto : IMapFrom<Domain.Models.CourseEvent>
{
    public string CourseEventId { get; set; }
        
    public string EventName { get; set; }

    public DateTime EventTime { get; set; }

    public CourseEventType EventType { get; set; }

    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.CourseEvent, CourseEventShortDto>();
    }
}