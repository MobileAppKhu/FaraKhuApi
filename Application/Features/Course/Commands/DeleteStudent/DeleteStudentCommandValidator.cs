using Application.Features.Course.Commands.DeleteStudent;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Commands.DeleteStudent
{
    public class DeleteStudentCommandValidator : AbstractValidator<DeleteStudentCommand>
    {
        public DeleteStudentCommandValidator(IStringLocalizer<SharedResource> localizer)
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