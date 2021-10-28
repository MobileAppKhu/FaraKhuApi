using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.AddQuestion
{
    public class AddQuestionCommandValidator : AbstractValidator<AddQuestionCommand>
    {
        public AddQuestionCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            /*RuleFor(r => r.CourseId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.MultiVote)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
            RuleFor(r => r.QuestionDescription)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);*/
        }
    }
}