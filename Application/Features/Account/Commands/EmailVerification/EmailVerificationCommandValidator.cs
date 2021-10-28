using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.EmailVerification
{
    public class EmailVerificationCommandValidator : AbstractValidator<EmailVerificationCommand>
    {
        public EmailVerificationCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"])
                .Must(e => e.IsEmail())
                .WithMessage(localizer["InvalidEmail"]);
        }
    }
}