using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Ticket;
using Application.Resources;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Queries.SearchTicket
{
    public class SearchTicketQueryHandler : IRequestHandler<SearchTicketQuery, SearchTicketQueryViewModel>
    {
        private readonly IDatabaseContext _context;
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }

        public SearchTicketQueryHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            Localizer = localizer;
        }

        public async Task<SearchTicketQueryViewModel> Handle(SearchTicketQuery request,
            CancellationToken cancellationToken)
        {
            // validation ?
            
            List<Domain.Models.Ticket> tickets = await _context.Tickets.Where(ticket =>
                request.TicketIds.Contains(ticket.TicketId) &&
                request.TicketPriorities.Contains(ticket.Priority) &&
                request.TicketStatusList.Contains(ticket.Status))
                .Skip(request.Start)
                .Take(request.Step)
                .ToListAsync(cancellationToken);

            switch (request.TicketColumn)
            {
                case TicketColumn.TicketId :
                    tickets = request.OrderDirection ? tickets.OrderBy(ticket => ticket.TicketId).ToList() : tickets.OrderByDescending(ticket => ticket.TicketId).ToList();
                    break;
                case TicketColumn.Description :
                    tickets = request.OrderDirection ? tickets.OrderBy(ticket => ticket.Description).ToList() : tickets.OrderByDescending(ticket => ticket.Description).ToList();
                    break;
                case TicketColumn.Priority :
                    tickets = request.OrderDirection ? tickets.OrderBy(ticket => ticket.Priority).ToList() : tickets.OrderByDescending(ticket => ticket.Priority).ToList();
                    break;
                case TicketColumn.Status :
                    tickets = request.OrderDirection ? tickets.OrderBy(ticket => ticket.Status).ToList() : tickets.OrderByDescending(ticket => ticket.Status).ToList();
                    break;
                case TicketColumn.DeadLine :
                    tickets = request.OrderDirection ? tickets.OrderBy(ticket => ticket.DeadLine).ToList() : tickets.OrderByDescending(ticket => ticket.DeadLine).ToList();
                    break;
            }

            return new SearchTicketQueryViewModel
            {
                TicketDtos = _mapper.Map<List<TicketDto>>(tickets)
            };
        }
    }
}