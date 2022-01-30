using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.ResetPasswordValidation
{
    public class ResetPasswordValidationCommandValidator : AbstractValidator<ResetPasswordValidationCommand>
    {
        public ResetPasswordValidationCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"])
                .Must(e => e.IsEmail());
            RuleFor(r => r.Token)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"])
                .Length(5);
        }
    }
}