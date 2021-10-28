using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchProfile
{
    public class SearchProfileQueryValidator : AbstractValidator<SearchProfileQuery>
    {
        public SearchProfileQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}