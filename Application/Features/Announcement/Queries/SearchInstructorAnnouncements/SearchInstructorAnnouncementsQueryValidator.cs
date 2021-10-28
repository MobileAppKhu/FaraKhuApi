using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Announcement.Queries.SearchInstructorAnnouncements
{
    public class SearchInstructorAnnouncementsQueryValidator : AbstractValidator<SearchInstructorAnnouncementsQuery>
    {
        public SearchInstructorAnnouncementsQueryValidator(IStringLocalizer<SharedResource> localizer)
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