using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Command.UpdateAvatar
{
    public class UpdateAvatarCommandValidator : AbstractValidator<UpdateAvatarCommand>
    {
        public UpdateAvatarCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            
        }
    }
}