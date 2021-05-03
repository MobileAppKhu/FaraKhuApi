using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Suggestion.Command.CreateSuggestion
{
    public class CreateSuggestionCommandValidator : AbstractValidator<CreateSuggestionCommand>
    {
        public CreateSuggestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.Detail)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}