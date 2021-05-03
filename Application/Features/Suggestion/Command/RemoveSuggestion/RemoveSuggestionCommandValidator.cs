using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Command.RemoveSuggestion
{
    public class RemoveSuggestionCommandValidator : AbstractValidator<RemoveSuggestionCommand>
    {
        public RemoveSuggestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.SuggestionId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}