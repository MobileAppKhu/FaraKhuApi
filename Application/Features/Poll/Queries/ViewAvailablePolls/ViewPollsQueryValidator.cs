using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.ViewAvailablePolls
{
    public class ViewPollsQueryValidator : AbstractValidator<ViewPollsQuery>
    {
        public ViewPollsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}