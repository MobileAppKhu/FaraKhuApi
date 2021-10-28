using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.AddFavourite
{
    public class AddFavouriteCommandValidator : AbstractValidator<AddFavouriteCommand>
    {
        public AddFavouriteCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}