using System;
using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Commands.AddTicket
{
    public class AddTicketCommand : IRequest<AddTicketCommandViewModel>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime? DeadLine { get; set; }
    }
}