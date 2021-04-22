using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.SignUp
{
    public class SignUpCommandValidator : AbstractValidator<SignUpCommand>
    {
        public SignUpCommandValidator(IStringLocalizer localizer)
        {
            RuleFor(u => u.FirstName)
                .NotEmpty();

        }
    }
}