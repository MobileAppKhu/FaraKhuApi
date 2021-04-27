using FluentValidation;

namespace Application.Features.Course.Command.AddStudent
{
    public class AddStudentCommandValidator : AbstractValidator<AddStudentCommand>
    {
        public AddStudentCommandValidator()
        {
            RuleFor(r => r.CourseId)
                .NotEmpty();
            RuleFor(r => r.StudentId)
                .NotEmpty();
        }
    }
}