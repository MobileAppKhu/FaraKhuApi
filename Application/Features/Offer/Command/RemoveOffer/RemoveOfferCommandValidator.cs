using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Command.RemoveOffer
{
    public class RemoveOfferCommandValidator : AbstractValidator<RemoveOfferCommand>
    {
        public RemoveOfferCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.OfferId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}