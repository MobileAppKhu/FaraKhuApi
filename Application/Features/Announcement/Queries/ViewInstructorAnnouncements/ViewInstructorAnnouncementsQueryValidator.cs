﻿using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Queries.ViewInstructorAnnouncements
{
    public class ViewInstructorAnnouncementsQueryValidator : AbstractValidator<ViewInstructorAnnouncementsQuery>
    {
        public ViewInstructorAnnouncementsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Start)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
            RuleFor(r => r.Step)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}