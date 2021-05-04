using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.CreateQuestion
{
    public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
    {
        public CreateQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            
        }
    }
}