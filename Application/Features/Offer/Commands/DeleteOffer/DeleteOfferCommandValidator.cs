using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Commands.DeleteOffer;

public class DeleteOfferCommandValidator : AbstractValidator<DeleteOfferCommand>
{
    public DeleteOfferCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.OfferId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}