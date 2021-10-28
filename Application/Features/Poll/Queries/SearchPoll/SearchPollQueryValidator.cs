using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.SearchPoll
{
    public class SearchPollQueryValidator : AbstractValidator<SearchPollQuery>
    {
        public SearchPollQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}