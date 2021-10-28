using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Time.Commands.DeleteTime
{
    public class DeleteTimeCommandValidator : AbstractValidator<DeleteTimeCommand>
    {
        public DeleteTimeCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.TimeId)
                .NotEmpty();
        }
    }
}