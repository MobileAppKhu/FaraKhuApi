using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Commands.DeleteAnnouncement;

public class DeleteAnnouncementValidator : AbstractValidator<DeleteAnnouncementCommand>
{
    public DeleteAnnouncementValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(obj => obj.AnnouncementId)
            .NotEmpty().WithMessage("AnnouncementIdRequired");
    }
}