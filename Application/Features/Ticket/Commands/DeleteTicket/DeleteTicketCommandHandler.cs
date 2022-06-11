using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Features.Ticket.Commands.EditTicket;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Ticket.Commands.DeleteTicket
{
    public class DeleteTicketCommandHandler : IRequestHandler<DeleteTicketCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteTicketCommandHandler(IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            Localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == request.UserId, cancellationToken);

            var deletingTicket =
                await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.TicketId == request.TicketId, cancellationToken);

            if (deletingTicket == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.TicketNotFound,
                    Message = Localizer["TicketNotFound"]
                });
            }

            if (deletingTicket.CreatorId != user.Id && user.UserType != UserType.Owner)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            _context.Tickets.Remove(deletingTicket);
            await _context.SaveChangesAsync(cancellationToken);
            
            return Unit.Value;
        }
    }
}