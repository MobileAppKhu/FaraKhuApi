using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.RetractVote
{
    public class RetractVoteCommandValidator : AbstractValidator<RetractVoteCommand>
    {
        public RetractVoteCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.AnswerId)
                .NotEmpty()
                .WithMessage(localizer["EmptyInput"]);
        }
    }
}