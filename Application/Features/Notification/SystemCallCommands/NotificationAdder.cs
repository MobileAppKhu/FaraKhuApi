using Application.Common.Interfaces;
using Domain.BaseModels;
using Domain.Enum;

namespace Application.Features.Notification.SystemCallCommands
{
    public class NotificationAdder
    {
        public static async void AddNotification(IDatabaseContext context, string description, string entityId,
            NotificationObjectType objectType,
            NotificationOperation operation,
            BaseUser user)
        {
            await context.Notifications.AddAsync(new Domain.Models.Notification
            {
                Description = description,
                EntityId = entityId,
                User = user,
                UserId = user.Id,
                NotificationOperation = operation,
                NotificationObjectType = objectType
            });
        }
    }
}