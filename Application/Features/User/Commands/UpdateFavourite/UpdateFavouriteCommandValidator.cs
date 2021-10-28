using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.UpdateFavourite
{
    public class UpdateFavouriteCommandValidator : AbstractValidator<UpdateFavouriteCommand>
    {
        public UpdateFavouriteCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.FavouriteId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}