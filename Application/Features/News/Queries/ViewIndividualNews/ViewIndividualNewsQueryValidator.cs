using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.ViewIndividualNews
{
    public class ViewIndividualNewsQueryValidator : AbstractValidator<ViewIndividualNewsQuery>
    {
        public ViewIndividualNewsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NewsId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}