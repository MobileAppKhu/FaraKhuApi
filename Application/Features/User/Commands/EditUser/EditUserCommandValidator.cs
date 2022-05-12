using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.EditUser
{
    public class EditUserCommandValidator : AbstractValidator<EditUserCommand>
    {
        public EditUserCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}