using System.Collections.Generic;
using Application.DTOs.Notification;

namespace Application.Features.Notification.Queries.SearchNotification
{
    public class SearchNotificationViewModel
    {
        public List<NotificationSearchDto> Notifications { get; set; }
    }
}