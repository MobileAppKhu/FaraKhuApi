using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Time.Command.RemoveTime
{
    public class RemoveTimeCommandValidator : AbstractValidator<RemoveTimeCommand>
    {
        public RemoveTimeCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.TimeId)
                .NotEmpty();
        }
    }
}