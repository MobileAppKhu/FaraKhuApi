using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Account.Commands.EditProfile;

public class EditProfileCommandValidator : AbstractValidator<EditProfileCommand>
{
    public EditProfileCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
    }
}