using Application.Common.Mappings;

namespace Application.DTOs.Notification;

public class NotificationSearchDto : IMapFrom<Domain.Models.Notification>
{
    public string NotificationId { get; set; }
    public string Description { get; set; }
    public string EntityId { get; set; }
    public string NotificationObjectType { get; set; }
    public string NotificationOperation { get; set; }
}