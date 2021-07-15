using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Queries.ViewInstructorAnnouncements
{
    public class ViewInstructorAnnouncementsQueryValidator : AbstractValidator<ViewInstructorAnnouncementsQuery>
    {
        public ViewInstructorAnnouncementsQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            //TODO
        }
    }
}