using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.News.Queries.ViewNews
{
    public class ViewNewsQueryValidator : AbstractValidator<ViewNewsQuery>
    {
        public ViewNewsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Start)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.Step)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}