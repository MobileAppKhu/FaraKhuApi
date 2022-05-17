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

namespace Application.Features.Notification.Commands.DeleteNotification
{
    public class DeleteNotificationCommandHandler : IRequestHandler<DeleteNotificationCommand>
    {
        private readonly IDatabaseContext _context;
        private IStringLocalizer<SharedResource> Localizer { get; }

        public DeleteNotificationCommandHandler( IStringLocalizer<SharedResource> localizer,
            IDatabaseContext context)
        {
            _context = context;
            Localizer = localizer;
        }
        public async Task<Unit> Handle(DeleteNotificationCommand request, CancellationToken cancellationToken)
        {
            var notificationObj = await _context.Notifications
                .FirstOrDefaultAsync(
                    notification => notification.NotificationId == request.NotificationId &&
                                    notification.UserId == request.UserId, cancellationToken);

            if (notificationObj == null)
            {
                throw new CustomException(new Error
                {
                    ErrorType = ErrorType.NotificationNotFound,
                    Message = Localizer["NotificationNotFound"]
                });
            }

            _context.Notifications.Remove(notificationObj);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}