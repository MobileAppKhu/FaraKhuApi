using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Notification.Commands.DeleteNotification
{
    public class DeleteNotificationCommandValidator : AbstractValidator<DeleteNotificationCommand>
    {
        public DeleteNotificationCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NotificationId)
                .NotEmpty()
                .WithMessage("NotEmpty");
        }
    }
}