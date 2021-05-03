using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Command.RemoveUser
{
    public class RemoveUserCommandValidator : AbstractValidator<RemoveUserCommand>
    {
        public RemoveUserCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}