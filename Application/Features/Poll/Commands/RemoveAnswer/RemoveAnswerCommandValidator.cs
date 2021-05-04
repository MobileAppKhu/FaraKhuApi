using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.RemoveAnswer
{
    public class RemoveAnswerCommandValidator : AbstractValidator<RemoveAnswerCommand>
    {
        public RemoveAnswerCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            
        }
    }
}