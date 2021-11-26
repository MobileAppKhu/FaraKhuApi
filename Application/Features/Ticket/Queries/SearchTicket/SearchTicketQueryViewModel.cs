using System.Collections.Generic;
using Application.DTOs.Ticket;

namespace Application.Features.Ticket.Queries.SearchTicket
{
    public class SearchTicketQueryViewModel
    {
        public List<TicketDto> TicketDtos { get; set; }
        public int SearchLength { get; set; }
    }
}