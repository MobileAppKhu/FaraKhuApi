using System.Data;
using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Command.UpdateProfile
{
    public class UpdateProfileCommandValidator : AbstractValidator<UpdateProfileCommand>
    {
        public UpdateProfileCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(request => request.FirstName)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(request => request.LastName)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}