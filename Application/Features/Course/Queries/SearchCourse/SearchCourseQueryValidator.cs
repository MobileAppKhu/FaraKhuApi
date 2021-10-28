using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Course.Queries.SearchCourse
{
    public class SearchCourseQueryValidator : AbstractValidator<SearchCourseQuery>
    {
        public SearchCourseQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}