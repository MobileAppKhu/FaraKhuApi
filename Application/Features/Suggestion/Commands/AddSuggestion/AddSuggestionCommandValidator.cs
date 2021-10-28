using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Commands.AddSuggestion
{
    public class AddSuggestionCommandValidator : AbstractValidator<AddSuggestionCommand>
    {
        public AddSuggestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Detail)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}