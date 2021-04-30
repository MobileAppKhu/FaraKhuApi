using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Command.RemoveCourse
{
    public class RemoveCourseCommandValidator : AbstractValidator<RemoveCourseCommand>
    {
        public RemoveCourseCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}