using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.DeleteQuestion
{
    public class DeleteQuestionCommandValidator : AbstractValidator<DeleteQuestionCommand>
    {
        public DeleteQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
        }
    }
}