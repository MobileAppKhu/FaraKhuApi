using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.CourseEvent.Commands.AddCourseEvent
{
    public class AddCourseEventCommandValidator : AbstractValidator<AddCourseEventCommand>
    {
        public AddCourseEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.EventDescription)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.EventName)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.EventTime)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}