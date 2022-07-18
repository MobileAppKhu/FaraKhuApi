using Domain.BaseModels;
using Domain.Enum;

namespace Domain.Models;

public class Notification : BaseEntity
{
    public string NotificationId { get; set; }
    public string Description { get; set; }
    public string EntityId { get; set; }
    public NotificationObjectType NotificationObjectType { get; set; }
    public NotificationOperation NotificationOperation { get; set; }
    public string UserId { get; set; }
    public BaseUser User { get; set; }
}