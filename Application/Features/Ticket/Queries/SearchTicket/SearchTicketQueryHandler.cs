using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Ticket;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Ticket.Queries.SearchTicket;

public class SearchTicketQueryHandler : IRequestHandler<SearchTicketQuery, SearchTicketQueryViewModel>
{
    private readonly IDatabaseContext _context;
    private IHttpContextAccessor HttpContextAccessor { get; }
    private IMapper Mapper { get; }

    public SearchTicketQueryHandler(IHttpContextAccessor httpContextAccessor,
        IMapper mapper, IDatabaseContext context)
    {
        _context = context;
        HttpContextAccessor = httpContextAccessor;
        Mapper = mapper;
    }

    public async Task<SearchTicketQueryViewModel> Handle(SearchTicketQuery request,
        CancellationToken cancellationToken)
    {
        var userId = HttpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
        var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == userId,
            cancellationToken);

        IQueryable<Domain.Models.Ticket> ticketsQueryable = _context.Tickets
            .Include(ticket => ticket.Creator);

        if (!string.IsNullOrWhiteSpace(request.TicketId))
        {
            ticketsQueryable = ticketsQueryable.Where(ticket => ticket.TicketId.Contains(request.TicketId));
        }

        if (request.TicketPriority != 0)
        {
            ticketsQueryable = ticketsQueryable.Where(ticket => request.TicketPriority == ticket.Priority);
        }

        if (request.TicketStatus != 0)
        {
            ticketsQueryable = ticketsQueryable.Where(ticket => request.TicketStatus == ticket.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.Description))
        {
            ticketsQueryable = ticketsQueryable.Where(ticket => ticket.Description.Contains(request.Description));
        }

        switch (request.TicketColumn)
        {
            case TicketColumn.TicketId:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.TicketId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.TicketId);
                break;
            case TicketColumn.Description:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.Description)
                        .ThenBy(ticket => ticket.TicketId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.Description)
                        .ThenByDescending(ticket => ticket.TicketId);
                break;
            case TicketColumn.Priority:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.Priority)
                        .ThenBy(ticket => ticket.TicketId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.Priority)
                        .ThenByDescending(ticket => ticket.TicketId);
                break;
            case TicketColumn.Status:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.Status)
                        .ThenBy(ticket => ticket.TicketId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.Status)
                        .ThenByDescending(ticket => ticket.TicketId);
                break;
            case TicketColumn.DeadLine:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.DeadLine)
                        .ThenBy(ticket => ticket.TicketId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.DeadLine)
                        .ThenByDescending(ticket => ticket.TicketId);
                break;
            case TicketColumn.CreationDate:
                ticketsQueryable = request.OrderDirection
                    ? ticketsQueryable.OrderBy(ticket => ticket.CreatedDate)
                        .ThenBy(ticket => ticket.CreatorId)
                    : ticketsQueryable.OrderByDescending(ticket => ticket.CreatedDate)
                        .ThenByDescending(ticket => ticket.CreatorId);
                break;
        }

        int searchLength = await ticketsQueryable.CountAsync(cancellationToken);

        List<Domain.Models.Ticket> tickets = await
            ticketsQueryable.Skip(request.Start).Take(request.Step).ToListAsync(cancellationToken);
        return new SearchTicketQueryViewModel
        {
            TicketDtos = Mapper.Map<List<TicketDto>>(tickets),
            SearchLength = searchLength
        };
    }
}