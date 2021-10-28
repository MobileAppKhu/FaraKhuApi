using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Commands.DeleteCourseEvent
{
    public class DeleteCourseEventCommandValidator : AbstractValidator<DeleteCourseEventCommand>
    {
        public DeleteCourseEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseEventId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}