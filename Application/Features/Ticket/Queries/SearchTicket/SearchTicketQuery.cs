using System;
using System.Collections.Generic;
using Domain.Enum;
using MediatR;

namespace Application.Features.Ticket.Queries.SearchTicket
{
    public class SearchTicketQuery : IRequest<SearchTicketQueryViewModel>
    {
        public List<String> TicketIds { get; set; }
        public List<TicketPriority> TicketPriorities { get; set; }
        public List<TicketStatus> TicketStatusList { get; set; }
        public int Start { get; set; }
        public int Step { get; set; }
        public TicketColumn TicketColumn { get; set; }
        public bool OrderDirection { get; set; }
    }
}