using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Notification.Commands.DeleteNotification;

public class DeleteNotificationCommand : IRequest<Unit>
{
    [JsonIgnore] public string UserId { get; set; }
    public string NotificationId { get; set; }
}