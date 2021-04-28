using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Queries.ViewCourse
{
    public class ViewCourseQueryValidator : AbstractValidator<ViewCourseQuery>
    {
        public ViewCourseQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty();
        }
    }
}