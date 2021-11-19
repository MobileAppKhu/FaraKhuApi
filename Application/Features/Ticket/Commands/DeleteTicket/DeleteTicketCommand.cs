using MediatR;

namespace Application.Features.Ticket.Commands.DeleteTicket
{
    public class DeleteTicketCommand : IRequest<Unit>
    {
        public string TicketId { get; set; }
    }
}