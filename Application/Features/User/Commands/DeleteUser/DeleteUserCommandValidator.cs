using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Commands.DeleteUser
{
    public class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
    {
        public DeleteUserCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.UserId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}