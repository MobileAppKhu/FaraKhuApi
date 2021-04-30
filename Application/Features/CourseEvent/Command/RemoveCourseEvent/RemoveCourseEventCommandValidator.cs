using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Command.RemoveCourseEvent
{
    public class RemoveCourseEventCommandValidator : AbstractValidator<RemoveCourseEventCommand>
    {
        public RemoveCourseEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseEventId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}