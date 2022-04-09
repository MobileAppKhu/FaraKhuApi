using System.Text.Json.Serialization;
using Domain.Enum;
using MediatR;

namespace Application.Features.Notification.Commands.AddCourseNotification
{
    public class AddCourseNotificationCommand : IRequest<AddCourseNotificationViewModel>
    {
        [JsonIgnore] public string UserId { get; set; }
        public string Description { get; set; }
        public string CourseId { get; set; }
    }
}