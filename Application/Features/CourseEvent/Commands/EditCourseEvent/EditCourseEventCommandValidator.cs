using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Commands.EditCourseEvent
{
    public class EditCourseEventCommandValidator : AbstractValidator<EditCourseEventCommand>
    {
        public EditCourseEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseEventId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}