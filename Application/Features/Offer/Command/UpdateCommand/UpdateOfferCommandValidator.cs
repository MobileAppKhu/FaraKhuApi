using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Offer.Command.UpdateCommand
{
    public class UpdateOfferCommandValidator : AbstractValidator<UpdateOfferCommand>
    {
        public UpdateOfferCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.OfferId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}