using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.AddAnswer
{
    public class AddAnswerCommandValidator : AbstractValidator<AddAnswerCommand>
    {
        public AddAnswerCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.AnswerDescription)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.QuestionId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}