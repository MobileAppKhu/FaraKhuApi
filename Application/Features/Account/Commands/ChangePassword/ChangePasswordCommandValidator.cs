using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.NewPassword)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.OldPassword)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"])
                .NotEqual(r => r.NewPassword)
                .WithMessage(localizer["DuplicatePassword"]);
        }
    }
}