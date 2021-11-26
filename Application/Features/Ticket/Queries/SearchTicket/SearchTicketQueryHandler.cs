using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.DTOs.Ticket;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
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
        private IMapper Mapper { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }

        public SearchTicketQueryHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            Mapper = mapper;
            Localizer = localizer;
        }

        public async Task<SearchTicketQueryViewModel> Handle(SearchTicketQuery request,
            CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == userId,
                cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            IQueryable<Domain.Models.Ticket> ticketsQueryable = _context.Tickets
                .Include(ticket => ticket.Creator);

            if (request.TicketIds.Count != 0)
            {
                ticketsQueryable = ticketsQueryable.Where(ticket => request.TicketIds.Contains(ticket.TicketId));
            }

            if (request.TicketPriorities.Count != 0)
            {
                ticketsQueryable = ticketsQueryable.Where(ticket => request.TicketPriorities.Contains(ticket.Priority));
            }

            if (request.TicketStatusList.Count != 0)
            {
                ticketsQueryable = ticketsQueryable.Where(ticket => request.TicketStatusList.Contains(ticket.Status));
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
}