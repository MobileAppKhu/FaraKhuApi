using System;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Interfaces;
using Application.DTOs.Ticket;
using AutoMapper;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.Ticket.Commands.AddTicket;

public class AddTicketCommandHandler : IRequestHandler<AddTicketCommand, AddTicketCommandViewModel>
{
    private readonly IDatabaseContext _context;
    private IMapper _mapper { get; }

    public AddTicketCommandHandler(IMapper mapper, IDatabaseContext context)
    {
        _context = context;
        _mapper = mapper;
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