using System;
using Application.Common.Mappings;
using AutoMapper;

namespace Application.DTOs.Ticket
{
    public class TicketDto : IMapFrom<Domain.Models.Ticket>
    {
        public string TicketId { get; set; }
        public string Description { get; set; }
        public string Status { get; set; }
        public string Priority { get; set; }
        public DateTime DeadLine { get; set; }
        public string Creator { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<Domain.Models.Ticket, TicketDto>()
                .ForMember(s => s.Status,
                    opt =>
                        opt.MapFrom(src => src.Status.ToString()))
                .ForMember(s => s.Priority,
                    opt =>
                        opt.MapFrom(src => src.Priority.ToString()))
                .ForMember(s => s.Creator,
                    opt =>
                        opt.MapFrom(src => src.Creator.FirstName + "  " + src.Creator.LastName));
        }
    }
}