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
        private IHttpContextAccessor HttpContextAccessor { get; }
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteTicketCommandHandler(IHttpContextAccessor httpContextAccessor,
            IDatabaseContext context, IStringLocalizer<SharedResource> localizer)
        {
            _context = context;
            HttpContextAccessor = httpContextAccessor;
            Localizer = localizer;
        }

        public async Task<Unit> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _context.BaseUsers.FirstOrDefaultAsync(baseUser => baseUser.Id == userId, cancellationToken);
            if (user == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            }

            var deletingTicket =
                await _context.Tickets.FirstOrDefaultAsync(ticket => ticket.TicketId == request.TicketId, cancellationToken);

            if (deletingTicket.CreatorId != userId && user.UserType != UserType.Owner)
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