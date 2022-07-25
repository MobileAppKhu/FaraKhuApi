using System.Text.Json.Serialization;
using MediatR;

namespace Application.Features.Notification.Queries.SearchNotification;

public class SearchNotificationQuery : IRequest<SearchNotificationViewModel>
{
    [JsonIgnore] public string UserId { get; set; }
}