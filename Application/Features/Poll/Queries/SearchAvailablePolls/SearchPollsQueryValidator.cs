using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.SearchAvailablePolls;

public class SearchPollsQueryValidator : AbstractValidator<SearchPollsQuery>
{
    public SearchPollsQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.CourseId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}