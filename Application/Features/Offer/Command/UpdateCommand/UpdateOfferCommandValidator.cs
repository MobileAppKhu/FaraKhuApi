using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Command.UpdateCommand
{
    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Title)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.OfferType)
                .NotEmpty()
                .WithMessage(localizer["EmptyInout"]);
            RuleFor(r => r.Price)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}