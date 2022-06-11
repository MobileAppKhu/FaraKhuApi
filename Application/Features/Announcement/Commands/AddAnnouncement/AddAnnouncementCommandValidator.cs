using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Commands.AddAnnouncement
{
    public class AddAnnouncementCommandValidator : AbstractValidator<AddAnnouncementCommand>
    {
        public AddAnnouncementCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Title)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.Description)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}
