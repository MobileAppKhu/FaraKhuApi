using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.ResetPassword;

public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
{
    public ResetPasswordCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.Email)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"])
            .Must(e => e.IsEmail())
            .WithMessage(localizer["InvalidEmail"]);
        RuleFor(r => r.NewPassword)
            .NotEmpty()
            .WithMessage(localizer["PasswordRequired"])
            .MinimumLength(6)
            .WithMessage(localizer["PasswordLength"]);
    }
}