using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Commands.RemoveAnnouncement
{
    public class RemoveAnnouncementValidator : AbstractValidator<RemoveAnnouncementCommand>
    {
        public RemoveAnnouncementValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(obj => obj.AnnouncementId)
                .NotEmpty().WithMessage("AnnouncementIdRequired");
        }
    }
}
