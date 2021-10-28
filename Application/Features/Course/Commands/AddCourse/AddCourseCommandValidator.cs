using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.AddCourse
{
    public class AddCourseCommandValidator : AbstractValidator<AddCourseCommand>
    {
        public AddCourseCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseTitle)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}