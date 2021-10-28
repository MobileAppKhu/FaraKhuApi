using Application.Resources;
using Application.Utilities;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.AddUser
{
    public class AddUserCommandValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(u => u.FirstName)
                .NotEmpty()
                .WithMessage(localizer["FirstnameRequired"]);
            RuleFor(u => u.LastName)
                .NotEmpty()
                .WithMessage(localizer["LastnameRequired"]);
            RuleFor(u => u.Password)
                .NotEmpty()
                .WithMessage(localizer["PasswordRequired"])
                .MinimumLength(6)
                .WithMessage(localizer["MessageLength"]);
            RuleFor(u => u.Email)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"])
                .Must(e => e.IsEmail())
                .WithMessage(localizer["InvalidEmail"]);
        }
    }
}