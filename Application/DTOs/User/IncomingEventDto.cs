using System.Collections.Generic;
using Application.Common.Mappings;
using Application.DTOs.Event.PersonalEvent;
using AutoMapper;

namespace Application.DTOs.User;

public class IncomingEventDto : IMapFrom<Domain.BaseModels.BaseUser>
{
    public ICollection<EventShortDto> Events { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.BaseModels.BaseUser, IncomingEventDto>()
            .ForMember(e => e.Events,
                opt =>
                    opt.MapFrom(src => src.Events));
    }
}