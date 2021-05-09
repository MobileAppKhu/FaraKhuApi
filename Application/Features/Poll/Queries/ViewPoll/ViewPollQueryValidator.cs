using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Queries.ViewPoll
{
    public class ViewPollQueryValidator : AbstractValidator<ViewPollQuery>
    {
        public ViewPollQueryValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}