using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Event.PersonalEvent;

public class EventDto : IMapFrom<Domain.Models.Event>
{
    public string EventId { get; set; }
        
    public string EventName { get; set; }
        
    public string EventDescription { get; set; }
        
    public DateTime EventTime { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Event, EventDto>();
    }
}