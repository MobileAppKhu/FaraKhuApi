using Application.Features.Course.Command.RemoveStudent;
using FluentValidation;

namespace Application.Features.Course.Command.RemoveStudent
{
    public class RemoveStudentCommandValidator : AbstractValidator<RemoveStudentCommand>
    {
        public RemoveStudentCommandValidator()
        {
            RuleFor(r => r.CourseId)
                .NotEmpty();
            RuleFor(r => r.StudentId)
                .NotEmpty();
        }
    }
}