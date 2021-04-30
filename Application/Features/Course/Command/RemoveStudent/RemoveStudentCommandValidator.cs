using Application.Features.Course.Command.RemoveStudent;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Command.RemoveStudent
{
    public class RemoveStudentCommandValidator : AbstractValidator<RemoveStudentCommand>
    {
        public RemoveStudentCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.StudentId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}