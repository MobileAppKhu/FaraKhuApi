using System;
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

namespace Application.Features.Ticket.Commands.AddTicket
{
    public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, AddTicketCommandViewModel>
    {
        private readonly IDatabaseContext _context;
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IMapper _mapper { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }

        public AddTicketCommandHandler(IHttpContextAccessor httpContextAccessor,
            IMapper mapper, IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            Localizer = localizer;
        }

        public async Task<AddTicketCommandViewModel> Handle(AddTicketCommand request,
            CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }
            //if deadline is null should do sth :))
            var ticket = new Domain.Models.Ticket
            {
                Description = request.Description,
                Priority = request.Priority,
                Status = TicketStatus.Init,
                DeadLine = request.DeadLine,
                CreatorId = userId,
                Creator = user
            };
            await _context.Tickets.AddAsync(ticket, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return new AddTicketCommandViewModel
            {
                Ticket = _mapper.Map<TicketDto>(ticket)
            };
        }
    }
}