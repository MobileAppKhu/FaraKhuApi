using MediatR;

namespace Application.Features.Notification.Commands.DeleteNotification
{
    public class DeleteNotificationCommand : IRequest<Unit>
    {
        public string NotificationId { get; set; }
    }
}