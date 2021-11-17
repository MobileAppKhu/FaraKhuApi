using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Commands.EditTicket
{
    public class EditTicketCommand : IRequest<Unit>
    {
        public string TicketId { get; set; }
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime DeadLine { get; set; }
    }
}