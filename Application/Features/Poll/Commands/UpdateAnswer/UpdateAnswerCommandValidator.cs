using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.UpdateAnswer
{
    public class UpdateAnswerCommandValidator : AbstractValidator<UpdateAnswerCommand>
    {
        public UpdateAnswerCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.AnswerDescription)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.AnswerId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}