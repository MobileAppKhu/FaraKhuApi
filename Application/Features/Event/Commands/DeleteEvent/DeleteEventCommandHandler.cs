using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Application.Common.Exceptions;
using Application.Common.Interfaces;
using Application.Resources;
using Domain.BaseModels;
using Domain.Enum;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Commands.DeleteEvent;

public class DeleteEventCommandHandler : IRequestHandler<DeleteEventCommand>
{
    private readonly IDatabaseContext _context;
    private IStringLocalizer<SharedResource> Localizer { get; }
        

    public DeleteEventCommandHandler(IStringLocalizer<SharedResource> localizer, IDatabaseContext context)
    {
        _context = context;
        Localizer = localizer;
    }

    public async Task<Unit> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        BaseUser user = _context.BaseUsers.FirstOrDefault(u => u.Id == request.UserId);
            
                
        var eventObj = _context.Events
            .Include(e => e.User)
            .FirstOrDefault(e => e.EventId == request.EventId);
        if (eventObj == null)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.EventNotFound,
                Message = Localizer["EventNotFound"]
            });
        }

        if (eventObj.User != user && user.UserType != UserType.Owner)
        {
            throw new CustomException(new Error
            {
                ErrorType = ErrorType.Unauthorized,
                Message = Localizer["Unauthorized"]
            });
        }
        _context.Events.Remove(eventObj);
        await _context.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}