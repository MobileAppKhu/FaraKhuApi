using System;
using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Commands.EditTicket
{
    public class EditTicketCommand : IRequest<Unit>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string TicketId { get; set; }
        public string Description { get; set; }
        public TicketPriority? Priority { get; set; }
        public DateTime? DeadLine { get; set; }
        public TicketStatus? TicketStatus { get; set; }
    }
}