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
        private IMapper _mapper { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }

        public AddTicketCommandHandler(IMapper mapper, IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            _mapper = mapper;
            Localizer = localizer;
        }

        public async Task<AddTicketCommandViewModel> Handle(AddTicketCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(u => u.Id == request.UserId, cancellationToken);
            //check if this works
            var ticket = new Domain.Models.Ticket
            {
                Description = request.Description,
                Priority = request.Priority,
                Status = TicketStatus.Init,
                DeadLine = request.DeadLine,
                CreatorId = user.Id,
                Creator = user,
                CreatedDate = DateTime.Now
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