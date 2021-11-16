using System;
using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Commands.AddTicket
{
    public class AddTicketCommand : IRequest<AddTicketCommandViewModel>
    {
        public string Description { get; set; }
        public TicketPriority Priority { get; set; }
        public DateTime DeadLine { get; set; }
    }
}