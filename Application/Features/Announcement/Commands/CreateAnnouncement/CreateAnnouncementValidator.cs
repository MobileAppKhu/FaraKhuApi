using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Features.Announcement.Commands.CreateAnnouncement
{
    public class CreateAnnouncementValidator : AbstractValidator<CreateAnnouncementCommand>
    {
        public CreateAnnouncementValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Title)
                .NotEmpty().WithMessage(localizer["TitleRequired"]);
            RuleFor(r => r.Description)
                .NotEmpty().WithMessage(localizer["DescriptionRequired"]);
            RuleFor(r => r.Faculty)
                .NotEmpty().WithMessage(localizer["FacultyRequired"]);
            RuleFor(r => r.Department)
                .NotEmpty().WithMessage(localizer["DepartmentRequired"]);
        }
    }
}
