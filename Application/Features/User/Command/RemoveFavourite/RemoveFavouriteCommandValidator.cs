using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Command.RemoveFavourite
{
    public class RemoveFavouriteCommandValidator : AbstractValidator<RemoveFavouriteCommand>
    {
        public RemoveFavouriteCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.FavouriteId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}