using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.AddCourse;

public class AddCourseCommandValidator : AbstractValidator<AddCourseCommand>
{
    public AddCourseCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.CourseTypeId)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
        RuleFor(r => r.EndDate)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
        RuleFor(r => r.AddStudentDto)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
        RuleFor(r => r.AddStudentDto)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
        RuleFor(r => r.AvatarId)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
        RuleFor(r => r.Address)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
    }
}