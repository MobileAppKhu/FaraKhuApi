using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Commands.EditOffer;

public class EditOfferCommandValidator : AbstractValidator<EditOfferCommand>
{
    public EditOfferCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.OfferId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}