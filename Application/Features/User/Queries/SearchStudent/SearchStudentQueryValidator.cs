using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.User.Queries.SearchStudent;

public class SearchStudentQueryValidator : AbstractValidator<SearchStudentQuery>
{
    public SearchStudentQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.StudentId)
            .NotEmpty()
            .WithMessage(localizer["NotEmpty"]);
    }
}