using System;
using Application.Common.Mappings;
using AutoMapper;
using Domain.Enum;

namespace Application.DTOs.Ticket;

public class TicketDto : IMapFrom<Domain.Models.Ticket>
{
    public string TicketId { get; set; }
    public string Description { get; set; }
    public TicketStatus Status { get; set; }
    public string StatusToString { get; set; }
    public string PriorityToString { get; set; }
    public TicketPriority Priority { get; set; }
    public DateTime DeadLine { get; set; }
    public string Creator { get; set; }
    public DateTime CreatedDate { get; set; }
    public void Mapping(Profile profile)
    {
        profile.CreateMap<Domain.Models.Ticket, TicketDto>()
            .ForMember(s => s.StatusToString,
                opt =>
                    opt.MapFrom(src => src.Status.ToString()))
            .ForMember(s => s.PriorityToString,
                opt =>
                    opt.MapFrom(src => src.Priority.ToString()))
            .ForMember(s => s.Creator,
                opt =>
                    opt.MapFrom(src => src.Creator.FirstName + "  " + src.Creator.LastName));
    }
}