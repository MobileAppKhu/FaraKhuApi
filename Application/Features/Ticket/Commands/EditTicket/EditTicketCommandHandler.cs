using System;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Notification.SystemCallCommands;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Commands.EditTicket
{
    public class EditTicketCommandHandler : IRequestHandler<EditTicketCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public EditTicketCommandHandler(IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(EditTicketCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == request.UserId, cancellationToken);

            var editingTicket =
                await _context.Tickets.Include(ticket => ticket.Creator)
                    .FirstOrDefaultAsync(ticket => ticket.TicketId == request.TicketId, cancellationToken);

            if (editingTicket == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.TicketNotFound,
                    Message = Localizer["TicketNotFound"]
                });
            }
            
            if (user.Id != editingTicket.CreatorId && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            if (!String.IsNullOrWhiteSpace(request.Description))
            {
                editingTicket.Description = request.Description;
            }

            if (request.Priority != null)
            {
                editingTicket.Priority = (TicketPriority)request.Priority;
            }

            if (request.DeadLine != null)
            {
                editingTicket.DeadLine = request.DeadLine;
            }

            if (request.TicketStatus != null)
            {
                if (user.UserType != UserType.Owner)
                {
                    throw new CustomException(new Error
                    {
                        ErrorType = ErrorType.Unauthorized,
                        Message = Localizer["Unauthorized"]
                    });
                }
                editingTicket.Status = (TicketStatus)request.TicketStatus;
                if (request.TicketStatus == TicketStatus.InProgress)
                {
                    NotificationAdder.AddNotification(_context, Localizer["YourTicketIsInProgress"],
                        editingTicket.TicketId, NotificationObjectType.Ticket, NotificationOperation.TicketImprovement,
                        editingTicket.Creator);
                }
                if (request.TicketStatus == TicketStatus.Solved)
                {
                    NotificationAdder.AddNotification(_context, Localizer["YourTicketHasBeenSolved"],
                        editingTicket.TicketId, NotificationObjectType.Ticket, NotificationOperation.TicketImprovement,
                        editingTicket.Creator);
                }
            }
            
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}