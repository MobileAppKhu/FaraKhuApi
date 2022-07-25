using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Queries.SearchTicket;

public class SearchTicketQuery : IRequest<SearchTicketQueryViewModel>
{
    public string TicketId { get; set; }
    public TicketPriority TicketPriority { get; set; }
    public TicketStatus TicketStatus { get; set; }
    public string Description { get; set; }
    public int Start { get; set; }
    public int Step { get; set; }
    public TicketColumn TicketColumn { get; set; }
    public bool OrderDirection { get; set; }
}