using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.SignIn
{
    public class SignInCommandValidator : AbstractValidator<SignInCommand>
    {
        public SignInCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(s => s.Logon)
                .NotEmpty();
            RuleFor(s => s.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordRequired"])
                .MinimumLength(6)
                .WithMessage(localizer["PasswordLength"]);
        }
    }
}