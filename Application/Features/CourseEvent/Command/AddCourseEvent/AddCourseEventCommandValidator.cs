using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Command.AddCourseEvent
{
    public class AddCourseEventCommandValidator : AbstractValidator<AddCourseEventCommand>
    {
        public AddCourseEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty();
            RuleFor(r => r.EventDescription)
                .NotEmpty();
            RuleFor(r => r.EventName)
                .NotEmpty();
            RuleFor(r => r.EventTime)
                .NotEmpty();
        }
    }
}