using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Application.Features.Announcement.Commands.DeleteAnnouncement;

namespace Application.Features.Announcement.Commands.DeleteAnnouncement
{
    public class DeleteAnnouncementValidator : AbstractValidator<DeleteAnnouncementCommand>
    {
        public DeleteAnnouncementValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(obj => obj.AnnouncementId)
                .NotEmpty().WithMessage("AnnouncementIdRequired");
        }
    }
}
