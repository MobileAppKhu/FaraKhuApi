using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.EditQuestion;

public class EditQuestionCommandValidator : AbstractValidator<EditQuestionCommand>
{
    public EditQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.QuestionId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}