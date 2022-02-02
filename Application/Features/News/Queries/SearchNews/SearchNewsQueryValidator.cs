using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.SearchNews
{
    public class SearchNewsQueryValidator : AbstractValidator<SearchNewsQuery>
    {
        public SearchNewsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            // ?
            // RuleFor(r => r.Start)
            //     .NotEmpty()
            //     .WithMessage(localizer["NotEmpty"]);
            // RuleFor(r => r.Step)
            //     .NotEmpty()
            //     .WithMessage(localizer["NotEmpty"]);
        }
    }
}