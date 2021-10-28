using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.SearchIndividualNews
{
    public class SearchIndividualNewsQueryValidator : AbstractValidator<SearchIndividualNewsQuery>
    {
        public SearchIndividualNewsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NewsId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}