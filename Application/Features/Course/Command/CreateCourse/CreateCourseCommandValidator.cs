using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Command.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseTitle)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}