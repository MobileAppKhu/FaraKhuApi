using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.Vote;

public class VoteCommandValidator : AbstractValidator<VoteCommand>
{
    public VoteCommandValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(r => r.AnswerId)
            .NotEmpty()
            .WithMessage(localizer["EmptyInput"]);
    }
}