using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.DeleteAnswer
{
    public class DeleteAnswerCommandValidator : AbstractValidator<DeleteAnswerCommand>
    {
        public DeleteAnswerCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.AnswerId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}