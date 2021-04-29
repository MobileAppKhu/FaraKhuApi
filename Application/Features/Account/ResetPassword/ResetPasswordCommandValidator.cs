using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Email)
                .NotEmpty()
                .Must(e => e.IsEmail());
            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .MinimumLength(6);
        }
    }
}