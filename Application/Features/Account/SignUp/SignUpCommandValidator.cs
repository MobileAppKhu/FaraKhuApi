using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage(localizer["FirstnameRequired"]);
            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage(localizer["LastnameRequired"]);
            RuleFor(u => u.Password)
                .NotEmpty()
                .MinimumLength(6)
                .WithMessage(localizer["MessageLength"]);
            RuleFor(u => u.Email)
                .NotEmpty()
                .Must(e => e.IsEmail())
                .WithMessage(localizer["EmailIncorrect"]);

        }
    }
}