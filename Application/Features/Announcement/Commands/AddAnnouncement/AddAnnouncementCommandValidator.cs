using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
