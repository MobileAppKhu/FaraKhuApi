using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Notification.Commands.AddCourseNotification
{
    public class AddCourseNotificationCommandValidator : AbstractValidator<AddCourseNotificationCommand>
    {
        public AddCourseNotificationCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}