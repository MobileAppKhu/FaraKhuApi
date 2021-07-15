using Application.Resources;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Application.Features.Event.Command.UpdateEvent
{
    public class UpdateEventCommandValidator : AbstractValidator<UpdateEventCommand>
    {
        public UpdateEventCommandValidator(IStringLocalizer<SharedResource> localizer)
        {
            RuleFor(r => r.EventId)
                .NotEmpty()
                .WithMessage(localizer["NotEmpty"]);
                
        }
    }
}