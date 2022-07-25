using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Commands.EditAnnouncement;

public class EditAnnouncementCommandValidator : AbstractValidator<EditAnnouncementCommand>
{
    public EditAnnouncementCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.AnnouncementId)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
    }
}