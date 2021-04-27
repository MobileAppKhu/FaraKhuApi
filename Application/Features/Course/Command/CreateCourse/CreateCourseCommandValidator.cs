using FluentValidation;

namespace Application.Features.Course.Command.CreateCourse
{
    public class CreateCourseCommandValidator : AbstractValidator<CreateCourseCommand>
    {
        public CreateCourseCommandValidator()
        {
            RuleFor(r => r.CourseTitle)
                .NotEmpty();
        }
    }
}