using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.UpdateQuestion
{
    public class UpdateQuestionCommandValidator : AbstractValidator<UpdateQuestionCommand>
    {
        public UpdateQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.QuestionDescription)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}