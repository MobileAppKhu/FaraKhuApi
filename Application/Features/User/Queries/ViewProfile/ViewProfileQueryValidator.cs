using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.ViewProfile
{
    public class ViewProfileQueryValidator : AbstractValidator<ViewProfileQuery>
    {
        public ViewProfileQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}