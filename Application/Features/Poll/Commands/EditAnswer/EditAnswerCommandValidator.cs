using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.EditAnswer
{
    public class EditAnswerCommandValidator : AbstractValidator<EditAnswerCommand>
    {
        public EditAnswerCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.AnswerId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}