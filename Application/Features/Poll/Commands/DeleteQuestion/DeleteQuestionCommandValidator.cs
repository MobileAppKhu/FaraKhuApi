using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.DeleteQuestion
{
    public class RemoveQuestionCommandValidator : AbstractValidator<RemoveQuestionCommand>
    {
        public RemoveQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}