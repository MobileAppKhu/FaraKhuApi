using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.EditCourse;

public class EditCourseCommandValidator : AbstractValidator<EditCourseCommand>
{
    public EditCourseCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.CourseId)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
    }
}