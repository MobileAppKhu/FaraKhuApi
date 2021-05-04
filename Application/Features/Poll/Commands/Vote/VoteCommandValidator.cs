using Application.Resources;
using Domain.Enum;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Poll.Commands.Vote
{
    public class VoteCommandValidator : AbstractValidator<VoteCommand>
    {
        public VoteCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            
        }
    }
}