using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;
using Domain.Models;

namespace Application.DTOs.Event.PersonalEvent
{
    public class EventShortDto : IMapFrom<Domain.Models.Event>
    {
        public string EventId { get; set; }
        public string EventName { get; set; }

        public DateTime EventTime { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Event, EventShortDto>();
        }
    }
}