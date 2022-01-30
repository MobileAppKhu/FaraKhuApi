using Domain.Enum;
using MediatR;

namespace Application.Features.Notification.Commands.AddCourseNotification
{
    public class AddCourseNotificationCommand : IRequest<AddCourseNotificationViewModel>
    {
        public string Description { get; set; }
        public string CourseId { get; set; }
    }
}