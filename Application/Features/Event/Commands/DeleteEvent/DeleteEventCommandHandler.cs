using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using AutoMapper;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Commands.DeleteEvent
{
    public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }
        private IHttpContextAccessor HttpContextAccessor { get; }

        public DeleteEventCommandHandler(IStringLocalizer<SharedResource> localizer,
            IHttpContextAccessor httpContextAccessor, IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
            HttpContextAccessor = httpContextAccessor;
        }

        public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
        {
            var userId = HttpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
            BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == userId);
            if (user == null)
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.Unauthorized,
                    Message = Localizer["Unauthorized"]
                });
            var eventObj = _context.Events.FirstOrDefault(e => e.EventId == request.EventId);
            if (eventObj != null)
                _context.Events.Remove(eventObj);
            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}